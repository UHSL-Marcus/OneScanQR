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
    /// Summary description for OneScanGetResult
    /// </summary>
    public class OneScanGetResult : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            int? status = 3;

            string doorID = context.Request.QueryString["door_id"];

            string toHmac = "door_id=" + doorID;
            string hmac = context.Request.QueryString["data"];

            List<Door> doors;
            if (SQLControls.getEntryByColumn(doorID, "DoorID", out doors) || doors.Count > 1)
            {
                string secret = doors[0].DoorSecret;
                if (HMAC.ValidateHash(toHmac, secret, hmac))
                {
                    string sessionID;
                    if (Global.OneScanSessions.TryGetValue(doorID, out sessionID))
                    {
                        byte[] reply;
                        if (HTTPRequest.HTTPGetRequest(ConfigurationManager.AppSettings["OnescanStatusCheckURL"] + "?OnescanSessionID=" + sessionID, out reply))
                        {
                            status = JsonUtils.GetObject<OneScanSessionStatus>(System.Text.Encoding.Default.GetString(reply)).Status;
                        }
                    }
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

    class OneScanSessionStatus
    {
        public int? Status;
    }
}