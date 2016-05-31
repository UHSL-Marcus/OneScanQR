using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceModel.WebSockets;

namespace WS.Server
{
    
    public class WSService : WebSocketService
    {
        
        public override void OnOpen()
        {
            Send("Welcome");
        }

        public override void OnMessage(string message)
        {
            string msgBack = string.Format(
            "You have sent {0} at {1}", message, DateTime.Now.ToLongTimeString());
            Send(msgBack);
        }

        protected override void OnClose()
        {
            base.OnClose();
        }       
    }
}
