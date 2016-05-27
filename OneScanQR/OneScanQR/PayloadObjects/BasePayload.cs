using System;

using Newtonsoft.Json;

namespace OneScanQR.PayloadObjects
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
            this.transactionID = transactionID;
            LoginPayload.LoginMode = LoginMode.ToString();
            if (LoginMode == LoginTypes.Register)
                LoginPayload.Profiles = new string[] { "basic" };  
           
        }

        public string GetJson(bool htmlEncode = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(this, settings);
            if (htmlEncode)
                json = Uri.EscapeUriString(json);

            return json;
        }
    }

    class MetaData
    {
        public string EndpointURL = "http://mmtsnap.mmt.herts.ac.uk/onescan/callback.aspx";
    }




}
