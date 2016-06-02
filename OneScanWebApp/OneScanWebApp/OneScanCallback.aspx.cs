using OneScanWebApp.PayloadObjects;
using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OneScanWebApp
{
    public partial class OneScanCallback1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Global.edited = DateTime.Now.ToLongDateString();
            

            /*if (Request.HttpMethod != "POST")
                return;

            Stream jsonStream = Request.InputStream;
            if (jsonStream.Length > int.MaxValue)
                return;

            int jsonLen = Convert.ToInt32(jsonStream.Length);
            jsonStream.Position = 0;
            RecievedLoginData LoginReply;

            using (StreamReader sr = new StreamReader(jsonStream))
            {
                string json = sr.ReadToEnd();
                string hmac = Request.Headers["x-onescan-signature"];
                //if (!HMAC.ValidateHash(json, ConfigurationManager.AppSettings["AuthSecret"], hmac))
                // return;


                LoginReply = JsonUtils.GetObject<RecievedLoginData>(json);

            }

            ProcessOutcomePayload outcome = new ProcessOutcomePayload();
            int success = 3;

            if (LoginReply.Success)
            {
                if (LoginReply.ProcessType.Equals(ProcessTypes.Login.ToString())
                    && LoginReply.MessageType.Equals(MessageTypes.StartLogin.ToString()))
                {
                    string token = LoginReply.UserToken.UserToken;
                    // check in database
                    Random rand = new Random();
                    if (rand.Next(21) > 10)
                    {
                        outcome.Success = true;
                        outcome.MessageType = OutcomeTypes.ProcessComplete.ToString();
                        success = 2;
                    }
                }
            }

            string reply;
            OneScanRequests.SendOneScanPayload(JsonUtils.GetJson(outcome), out reply);

            SessionData sd = JsonUtils.GetObject<SessionData>(LoginReply.SessionData);
            if (Global.OneScanSessionStatus.Keys.Contains(sd.sessionID))
                Global.OneScanSessionStatus[sd.sessionID] = success.ToString();*/
        }
    }
}