using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for bytearraytest
    /// </summary>
    public class bytearraytest : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            byte[] bytes;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://localhost/OneScanWebApp/onescanrequestsession.ashx?mode=0&qr_img=1&door_id=34&data=dfg");
            req.Method = "GET";

            WebResponse response = req.GetResponse();
            Stream responseStream = response.GetResponseStream();

            using (MemoryStream ms = new MemoryStream())
            {
                responseStream.CopyTo(ms);
                bytes = ms.ToArray();
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes) 
                sb.Append("0x" + b.ToString("X2") + ",");

            string hexString = sb.ToString();

            context.Response.ContentType = "text/plain";
            context.Response.Write(hexString);
            
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