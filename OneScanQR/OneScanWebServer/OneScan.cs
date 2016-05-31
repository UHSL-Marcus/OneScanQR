using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebServer
{
    public class OneScan
    {
        private readonly static Lazy<OneScan> _instance = new Lazy<OneScan>(() => new OneScan(GlobalHost.ConnectionManager.GetHubContext<OneScanHub>().Clients));

        private IHubConnectionContext<dynamic> Clients;

        private OneScan(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public static OneScan Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        
    }
}