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
            SessionData sData = JsonUtils.GetObject<SessionData>(LoginReply.SessionData);

            string sessionKey = sData.doorID;
            if (sData.regkey != null)
                sessionKey = sData.regkey;

            if (Global.OneScanSessions.ContainsKey(sessionKey))
            {
                if (LoginReply.Success)
                {
                    int? userTokenId;
                    SQLControls.getEntryIDByColumn<UserToken>(LoginReply.UserToken.UserToken, "Token", out userTokenId);

                    if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.UserToken.ToString()) && userTokenId != null)
                    {
                        int? doorId;
                        if (SQLControls.getEntryIDByColumn<Door>(sData.doorID, "DoorID", out doorId))
                        {
                            DoorUserTokenPair pair = new DoorUserTokenPair();
                            pair.DoorID = doorId;
                            pair.UserToken = userTokenId;

                            if (SQLControls.getEntryExists(pair))
                            {
                                outcome.Success = true;
                                outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
                            }
                        }
                    }

                    if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.Register.ToString()))
                    {
                        bool continueReg = false;

                        if (userTokenId == null)
                        {
                            UserToken ut = new UserToken();
                            ut.Token = LoginReply.UserToken.UserToken;
                            if (SQLControls.doInsertReturnID(ut, out userTokenId))
                            {
                                UserInfo u = new UserInfo();
                                u.Name = LoginReply.LoginCredentials.FirstName + " " + LoginReply.LoginCredentials.LastName;
                                u.UserToken = userTokenId;

                                if (SQLControls.doInsert(u))
                                    continueReg = true;
                            }
                        }
                        else continueReg = true;

                        if (continueReg)
                        {
                            outcome.Success = true;
                            outcome.MessageType = OutcomeTypes.ProcessComplete.ToString(); 
                        }
                    }
                }
            }

            string jsonResponse = JsonUtils.GetJson(outcome);

            context.Response.Headers.Add("Content-Type", "application/json");
            context.Response.Headers.Add("x-onescan-account", ConfigurationManager.AppSettings["AuthKey"]);
            string hmacResponse = HMAC.Hash(jsonResponse, ConfigurationManager.AppSettings["AuthSecret"]);
            context.Response.Headers.Add("x-onescan-signature", hmacResponse);

            using (StreamWriter sw = new StreamWriter(context.Response.OutputStream))
            {
                sw.Write(jsonResponse);
                context.Response.Flush();
            }

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