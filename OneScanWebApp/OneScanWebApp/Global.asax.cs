using OneScanWebApp.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace OneScanWebApp
{
    public class Global : System.Web.HttpApplication
    {
        private static ConcurrentDictionary<string, string> _onescanSessions;

        public static ConcurrentDictionary<string, string> OneScanSessions
        {
            get
            {
                if (_onescanSessions == null)
                {
                    _onescanSessions = new ConcurrentDictionary<string, string>();
                }
                return _onescanSessions;  
            }
        }

        private static ConcurrentDictionary<string, string> _onescanAdminSessions;

        public static ConcurrentDictionary<string, string> OneScanAdminSessions
        {
            get
            {
                if (_onescanAdminSessions == null)
                {
                    _onescanAdminSessions = new ConcurrentDictionary<string, string>();
                }
                return _onescanAdminSessions;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
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
            
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}