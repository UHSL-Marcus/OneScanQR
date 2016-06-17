using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for errortest
    /// </summary>
    public class errortest : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            ((Global)context.ApplicationInstance).UpdateLog("this is some info.com,'hello';");

            Database.Objects.Log log = new Database.Objects.Log();
            log.Guid = Guid.NewGuid().ToString();
            log.Timestamp = DateTime.UtcNow;
            log.Error = "test entry.com 'info;'@";

            int? output;
            SQLControls.Set.doInsertReturnID(log, out output);

            throw new HttpException(500, "inserted: " + output);

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