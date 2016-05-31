using System;

using Newtonsoft.Json;

namespace OneScanWebServer.Payload
{

    class BasePayload
    {
        public string ProcessType;
        public string MessageType;
        public string SessionData = "CUSTOM SESSION DATA";
        public int? Version = 2;
        public MetaData MetaData = new MetaData();
        public LoginPayload LoginPayload;

        [JsonIgnore]
        public string transactionID;

        public void SetLoginPayload(LoginTypes LoginMode, string transactionID)
        {
            LoginPayload = new LoginPayload();
            ProcessType = "Login";
            MessageType = "StartLogin";
            SessionData = transactionID;
            LoginPayload.LoginMode = LoginMode.ToString();
            if (LoginMode == LoginTypes.Register)
                LoginPayload.Profiles = new string[] { "basic" };  
           
        }

        public string GetJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this, settings);
        }
    }

    class MetaData
    {
        public string EndpointURL = "http://mmtsnap.mmt.herts.ac.uk/onescan/callback.aspx";
    }




}
