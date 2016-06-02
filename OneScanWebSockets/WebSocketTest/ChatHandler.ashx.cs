﻿using Microsoft.Web.WebSockets;
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

    public class MyWSHandler : WebSocketHandler
    {
        public override void OnOpen()
        {
            this.Send("Welcome from " + this.WebSocketContext.UserHostAddress);
        }
        public override void OnMessage(string message)
        {
            string msgBack = string.Format(
                "You have sent {0} at {1}", message, DateTime.Now.ToLongTimeString());
            this.Send(msgBack);
        }
        public override void OnClose()
        {
            base.OnClose();
        }
        public override void OnError()
        {
            base.OnError();
        }
    }
}