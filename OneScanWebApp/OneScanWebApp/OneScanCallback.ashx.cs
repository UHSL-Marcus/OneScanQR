using OneScanWebApp.Database;
using OneScanWebApp.Database.Objects;
using OneScanWebApp.PayloadObjects;
using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for OneScanCallback
    /// </summary>
    public class OneScanCallback : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
                return;

            Stream jsonStream = context.Request.InputStream;
            if (jsonStream.Length > int.MaxValue)
                return;

            int jsonLen = Convert.ToInt32(jsonStream.Length);
            jsonStream.Position = 0;
            RecievedLoginData LoginReply;

            using (StreamReader sr = new StreamReader(jsonStream))
            {
                string json = sr.ReadToEnd();
                string hmac = context.Request.Headers["x-onescan-signature"];
                //if (!HMAC.ValidateHash(json, ConfigurationManager.AppSettings["AuthSecret"], hmac))
                   // return;


                LoginReply = JsonUtils.GetObject<RecievedLoginData>(json);

            }

            ProcessOutcomePayload outcome = new ProcessOutcomePayload();


            if (LoginReply.Success)
            {

                int? userTokenId;
                if (!SQLControls.getEntryIDByColumn<UserToken>(LoginReply.UserToken.UserToken, "Token", out userTokenId)) {
                    UserToken ut = new UserToken();
                    ut.Token = LoginReply.UserToken.UserToken;
                    SQLControls.doInsertReturnID(ut, out userTokenId);
                }

                int? doorId;
                SQLControls.getEntryIDByColumn<Door>(LoginReply.SessionData, "DoorID", out doorId);

                if (doorId != null && userTokenId != null) {

                    DoorUserTokenPair pair = new DoorUserTokenPair();
                    pair.DoorID = doorId; pair.UserToken = userTokenId;

                    if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.UserToken.ToString()))
                    {
                        if (SQLControls.getEntryExists(pair))
                        {
                            outcome.Success = true;
                            outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
                        }

                    }

                    if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.Register.ToString()))
                    {

                        int? id;
                        if (!SQLControls.getEntryID(pair, out id))
                            SQLControls.doInsertReturnID(pair, out id);
                        if (id != null)
                        {
                            outcome.Success = true;
                            outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
                        }

                    }
                }
            }

            string reply;
            OneScanRequests.SendOneScanPayload(JsonUtils.GetJson(outcome), out reply);  
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}