﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using PushFrenzy.Server;

namespace PushFrenzy.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new ServiceRoute("connect", 
                new WebSocketServiceHostFactory(), 
                typeof(WSService)));
        }
       
    }
}