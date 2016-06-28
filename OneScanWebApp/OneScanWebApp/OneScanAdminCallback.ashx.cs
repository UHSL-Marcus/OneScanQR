using OneScanWebApp.Database.Objects;
using OneScanWebApp.PayloadObjects;
using OneScanWebApp.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for OneScanAdminCallback
    /// </summary>
    public class OneScanAdminCallback : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            ProcessOutcomePayload outcome = new ProcessOutcomePayload();

            if (context.Request.HttpMethod == "POST")
            {

                Stream jsonStream = context.Request.InputStream;
                if (jsonStream.Length < int.MaxValue)
                {

                    int jsonLen = Convert.ToInt32(jsonStream.Length);
                    jsonStream.Position = 0;
                    string json = "";

                    using (StreamReader sr = new StreamReader(jsonStream))
                        json = sr.ReadToEnd();

                    string hmac;
                    if (context.Request.Headers.TryGetValue("x-onescan-signature", out hmac) &&
                        HMAC.ValidateHash(json, ConfigurationManager.AppSettings["AuthSecret"], hmac))
                    {
                        RecievedLoginData LoginReply = JsonUtils.GetObject<RecievedLoginData>(json);
                        
                        if (Global.OneScanAdminSessions.ContainsKey(LoginReply.SessionData))
                        {
                            if (LoginReply.Success)
                            {

                                int? userTokenId;
                                SQLControlsLib.Get.doSelectIDByColumn<AdminToken, string, int?>(LoginReply.UserToken.UserToken, "UserToken", out userTokenId);

                                if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.UserToken.ToString()) && userTokenId != null)
                                {
                                    outcome.Success = true;
                                    outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
                                }
                                if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.Register.ToString()))
                                {
                                    bool continueReg = false;

                                    if (userTokenId == null)
                                    {
                                        AdminToken at = new AdminToken();
                                        at.UserToken = LoginReply.UserToken.UserToken;
                                        if (SQLControlsLib.Set.doInsertReturnID(at, out userTokenId))
                                        {
                                            AdminUser au = new AdminUser();
                                            au.Name = LoginReply.LoginCredentials.FirstName + " " + LoginReply.LoginCredentials.LastName;
                                            au.AdminToken = userTokenId;

                                            if (SQLControlsLib.Set.doInsert(au))
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
                            } // not success
                        } // not in session
                    } // bad HMAC
                } // too long
            } // not post

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