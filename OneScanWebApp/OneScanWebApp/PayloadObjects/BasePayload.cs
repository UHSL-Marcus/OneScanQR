using System;

using Newtonsoft.Json;

namespace OneScanWebApp.PayloadObjects
{

    class BasePayload
    {
        public string ProcessType;
        public string MessageType;
        public string SessionData;
        public int? Version = 2;
        public MetaData MetaData = new MetaData();
        public LoginPayload LoginPayload;

        public void SetLoginPayload(LoginTypes LoginMode, string SessionData)
        {
            LoginPayload = new LoginPayload();
            ProcessType = ProcessTypes.Login.ToString(); ;
            MessageType = MessageTypes.StartLogin.ToString(); ;
            this.SessionData = SessionData;

            LoginPayload.LoginMode = LoginMode.ToString();
            if (LoginMode == LoginTypes.Register)
            {
                MessageType = MessageTypes.Login.ToString();
                LoginPayload.Profiles = new string[] { "basic" };
            }
           
        }
    }

    enum MessageTypes
    {
        StartLogin,
        Login
    }

    enum ProcessTypes
    {
        Login
    }

    class MetaData
    {
        public string EndpointURL = "http://mmtsnap.mmt.herts.ac.uk/onescan/OneScanCallback.aspx";
    }

    class SessionData
    {
        public string doorID;
        public string sessionID;
    }




}
