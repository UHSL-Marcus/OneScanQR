using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
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
            context.Response.ContentType = "text/plain";
            string id = (string)context.Session[Consts.GLOBAL_STATUS_ID];

            string reply;
            if (!Global.OneScanSessionStatus.TryGetValue(id, out reply))
                reply = "0";

            string s = "This session (" + id + ") : " + reply;
            foreach (string sessionID in Global.OneScanSessionStatus.Keys)
            {
                s += "\n" + sessionID + " : " + Global.OneScanSessionStatus[sessionID];
            }

            if (Convert.ToInt32(reply) > 1)
                Global.OneScanSessionStatus[id] = "0";


            context.Response.Write(Global.edited); //"{status:" + context.Session[Consts.OS_REPLY_SUCCESS] + "}");
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}