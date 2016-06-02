using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for Sessiontest2
    /// </summary>
    public class Sessiontest2 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            /*string s = "";
            foreach (HttpSessionState session in Global.OneScanSessionStatus.Values)
            {
                s += session.SessionID + " : " + session["tester"] + "\n";
                session["tester"] = "edit";
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(s);*/
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