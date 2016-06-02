using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneScanWebApp.PayloadObjects
{
    class RecievedLoginData
    {
        public int? Version;
        public UserTokenPayload UserToken = new UserTokenPayload();
        public LoginPayload LoginPayload;
        public string MessageType;
        public string ProcessType;
        public bool PlayMode;
        public string SessionData;
        public bool Success;
    }

    class UserTokenPayload
    {
        public string UserToken;
    }
}