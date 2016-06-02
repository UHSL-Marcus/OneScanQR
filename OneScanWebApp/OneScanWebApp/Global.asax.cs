using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace OneScanWebApp
{
    public class Global : System.Web.HttpApplication
    {
        private static Dictionary<string, string> _onescanStatus;
        private static readonly object padlock = new object();

        public static string edited = "no";

        public static Dictionary<string, string> OneScanSessionStatus
        {
            get
            {
                lock (padlock)
                {
                    if (_onescanStatus == null)
                    {
                        _onescanStatus = new Dictionary<string, string>();
                    }
                    return _onescanStatus;
                }
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string id = Guid.NewGuid().ToString();
            Session[Consts.GLOBAL_STATUS_ID] = id;
            OneScanSessionStatus.Add(id, "0");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            OneScanSessionStatus.Remove((string)Session[Consts.GLOBAL_STATUS_ID]);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}