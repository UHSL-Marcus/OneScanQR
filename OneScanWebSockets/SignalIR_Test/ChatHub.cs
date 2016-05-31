using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalIR_Test
{
    public class chatMessage
    {
        public string name;
        public string message;
    }
    public class ChatHub : Hub
    {
        private readonly Chat _chat;
        public ChatHub() : this(Chat.Instance) { }
        public ChatHub(Chat chat)
        {
            _chat = chat;
        }

        public void Send(string message)
        {
            // Call the broadcastMessage method to update clients.
            chatMessage cm = new chatMessage();
            cm.name = Context.ConnectionId;
            cm.message = message;
            _chat.BroadcastMessage(cm);
        }

        public override Task OnConnected()
        {
            chatMessage cm = new chatMessage();
            cm.name = Context.ConnectionId;
            cm.message = "New Connection";

            _chat.BroadcastMessage(cm);
            return base.OnConnected();
        }
    }

    public class Chat
    {
        private readonly static Lazy<Chat> _instance = new Lazy<Chat>(() => new Chat(GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients));

        private IHubConnectionContext<dynamic> Clients;

        private Chat(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public static Chat Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void BroadcastMessage(chatMessage cm)
        {
            Clients.All.broadcastMessage(cm);
        }
    }
}