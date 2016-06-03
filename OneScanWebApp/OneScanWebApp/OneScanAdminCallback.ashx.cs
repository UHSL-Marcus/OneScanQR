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

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for OneScanAdminCallback
    /// </summary>
    public class OneScanAdminCallback : IHttpHandler
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
                SQLControls.getEntryIDByColumn<AdminToken>(LoginReply.UserToken.UserToken, "UserToken", out userTokenId);

                if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.UserToken.ToString()) && userTokenId != null)
                {
                    outcome.Success = true;
                    outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
                }
                if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.Register.ToString()))
                {
                    if (userTokenId == null)
                    {
                        AdminToken at = new AdminToken();
                        at.UserToken = LoginReply.UserToken.UserToken;
                        SQLControls.doInsertReturnID(at, out userTokenId);
                    }

                    if (userTokenId != null)
                    {
                        outcome.Success = true;
                        outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
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