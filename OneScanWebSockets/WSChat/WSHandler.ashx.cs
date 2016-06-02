using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using Microsoft.Web.WebSockets;

namespace WSChat
{
    /// <summary>
    /// Summary description for WSHandler
    /// </summary>
    public class WSHandler : IHttpHandler
    {
        List<MyWSHandler> connections = new List<MyWSHandler>();
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                MyWSHandler handler = new MyWSHandler();
                context.AcceptWebSocketRequest(handler);
                connections.Add(handler);
            }
        }

        public bool IsReusable { get { return false; } }

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