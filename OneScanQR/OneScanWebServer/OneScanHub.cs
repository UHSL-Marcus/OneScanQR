using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebServer
{
    public class OneScanHub : Hub
    {
        private readonly OneScan _oneScan;
        public OneScanHub() : this(OneScan.Instance) { }
        public OneScanHub(OneScan oneScan)
        {
            _oneScan = oneScan;
        }

        public void RequestLoginQR()
        {

        }
    }
}