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
    /// Summary description for OneScanCallback
    /// </summary>
    public class OneScanCallback : IHttpHandler, IRequiresSessionState
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
                        SessionData sData = JsonUtils.GetObject<SessionData>(LoginReply.SessionData);

                        string sessionKey = sData.doorID;
                        if (sData.regkey != null)
                            sessionKey = sData.regkey;

                        if (Global.OneScanSessions.ContainsKey(sessionKey))
                        {
                            if (LoginReply.Success)
                            {
                                int? userTokenId;
                                SQLControlsLib.Get.doSelectIDByColumn<UserToken, string, int?>(LoginReply.UserToken.UserToken, "Token", out userTokenId);

                                if (LoginReply.LoginPayload.LoginMode.Equals(LoginTypes.UserToken.ToString()) && userTokenId != null)
                                {
                                    int? doorId;
                                    if (SQLControlsLib.Get.doSelectIDByColumn<Door, string, int?>(sData.doorID, "DoorID", out doorId))
                                    {
                                        DoorUserTokenPair pair = new DoorUserTokenPair();
                                        pair.DoorID = doorId;
                                        pair.UserToken = userTokenId;

                                        if (SQLControlsLib.Get.doEntryExists(pair))
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
                                        if (SQLControlsLib.Set.doInsertReturnID(ut, out userTokenId))
                                        {
                                            UserInfo u = new UserInfo();
                                            u.Name = LoginReply.LoginCredentials.FirstName + " " + LoginReply.LoginCredentials.LastName;
                                            u.UserToken = userTokenId;

                                            if (SQLControlsLib.Set.doInsert(u))
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