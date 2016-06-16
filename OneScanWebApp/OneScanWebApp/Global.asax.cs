using OneScanWebApp.Database;
using OneScanWebApp.Database.Objects;
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
    public class Global : HttpApplication
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
            Session[Consts.ERROR_ID] = Guid.NewGuid().ToString();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Context.Items.Add("ex", Server.GetLastError());
            Server.ClearError();
            var handler = new ErrorPage();
            handler.ProcessRequest(Context);
            Response.End();  
        }

        protected void Session_End(object sender, EventArgs e)
        {
            
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public void UpdateLog(string _info)
        {
            string entry = "";
            string guid = (string)Session[Consts.ERROR_ID];

            SQLControls.getSingleColumnByColumn(guid, "Log", "Guid", "Error", out entry);

            entry += "\n\n" + _info;

            Log log = new Log();
            log.Guid = guid;
            log.Timestamp = DateTime.UtcNow;
            log.Error = entry;

            SQLControls.doUpdateOrInsert(log, "Guid");
 
        }

        
    }
}