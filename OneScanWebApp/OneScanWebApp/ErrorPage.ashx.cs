using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for ErrorPage
    /// </summary>
    public class ErrorPage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            Exception ex = (Exception)context.Items["ex"];

            string err = "No Error";
            if (ex != null)
                err = ex.ToString();

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            context.Response.Write(err);
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