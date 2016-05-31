using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalIR_Test
{
    public partial class signalRtest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            chatMessage cm = new chatMessage();
            cm.name = "Web";
            cm.message = "Message";
            Chat.Instance.BroadcastMessage(cm);
        }
    }
}