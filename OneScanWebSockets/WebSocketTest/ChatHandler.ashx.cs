using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSocketTest
{
    /// <summary>
    /// Summary description for ChatHandler
    /// </summary>
    public class ChatHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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