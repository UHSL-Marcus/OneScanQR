using OneScanWebApp.Database.Objects;
using OneScanWebApp.Utils;
using System;
using System.Collections.Concurrent;
using System.Web;

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
            SQLControlsLib.Settings.SetConnectionString(Properties.Settings.Default.Database.ToString());
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

        protected void Application_Error(object sender, EventArgs e) // can also send an email to admins here too.
        {
            Context.Items.Add("ex", Server.GetLastError());
            Server.ClearError();
            (new ErrorPage()).ProcessRequest(Context);
            Response.TrySkipIisCustomErrors = true;
            Context.ApplicationInstance.CompleteRequest(); 
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

            SQLControlsLib.Get.doSelectSingleColumnByColumn(guid, "Log", "Guid", "Error", out entry);

            entry += "\n\n" + _info;

            Log log = new Log();
            log.Guid = guid;
            log.Timestamp = DateTime.UtcNow;
            log.Error = entry;

            SQLControlsLib.Update.doUpdateOrInsert(log, "Guid", guid);
 
        }

        
    }
}