using OneScanWebApp.Database;
using OneScanWebApp.Database.Objects;
using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for OneScanAdminGetResult
    /// </summary>
    public class OneScanAdminGetResult : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
 
            int? status = 3;

            int mode;
            if (int.TryParse(context.Request.QueryString["mode"], out mode))
            {

                string guid = context.Request.QueryString["guid"];

                string toHmac = "mode=" + mode + "&guid=" + guid;
                string hmac = context.Request.QueryString["data"];

                if (guid != null && hmac != null)
                {

                    string secret = "";
                    string key = null;

                    switch (mode)
                    {
                        case 0:
                            secret = ConfigurationManager.AppSettings["AdminSecret"];
                            break;
                        case 1:
                            key = context.Request.QueryString["key"];
                            toHmac += "&key=" + key;
                            List<RegistrationToken> regtokns;
                            if (SQLControlsLib.Get.doSelectByColumn(key, "AuthKey", out regtokns) || regtokns.Count > 1)
                                secret = regtokns[0].Secret;
                            break;
                    }

                    //if (HMAC.ValidateHash(toHmac, secret, hmac))
                    //{
                        string sessionID;
                        if (Global.OneScanAdminSessions.TryGetValue(guid, out sessionID))
                        {
                            byte[] reply;
                            if (HTTPRequest.HTTPGetRequest(ConfigurationManager.AppSettings["OnescanStatusCheckURL"] + "?OnescanSessionID=" + sessionID, out reply))
                            {
                                status = JsonUtils.GetObject<OneScanSessionStatus>(System.Text.Encoding.Default.GetString(reply)).Status;
                                if (key != null && status > 1)
                                    SQLControlsLib.Delete.doDeleteEntryByColumn<RegistrationToken, string>(key, "AuthKey");
                            }
                        }
                    //}
                }

            }

            context.Response.Write(status);
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