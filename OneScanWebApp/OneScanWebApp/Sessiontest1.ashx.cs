using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for Sessiontest1
    /// </summary>
    public class Sessiontest1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (context.Session["tester"] == null)
                context.Session["tester"] = "nothing";

            context.Response.Write(context.Session["tester"]);
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