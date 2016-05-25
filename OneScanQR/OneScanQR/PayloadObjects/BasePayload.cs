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

        public void SetLoginPayload(LoginTypes LoginMode, bool fullPayload = true, LoginResponseTypes respType = LoginResponseTypes.LoginProblem)
        {
            LoginPayload = new LoginPayload();
            ProcessType = "Login";
            MessageType = "StartLogin";
            LoginPayload.LoginMode = LoginMode.ToString();
            if (LoginMode == LoginTypes.Register)
                LoginPayload.Profiles = new string[] { "basic" };  
            
            if (fullPayload)
            {
                BaseFullPayload tempFpl = new BaseFullPayload(); tempFpl.SetLoginFullPayload(LoginMode, respType);
                SessionData = tempFpl.GetJson();
            } 
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

    class BaseFullPayload
    {
        public BasePayload FullPayload;
        public string TransactionId = "tempID";
        public string LoginResponseType;

        public void SetLoginFullPayload(LoginTypes type, LoginResponseTypes respType)
        {
            FullPayload = new BasePayload();
            FullPayload.SetLoginPayload(type, false);

            LoginResponseType = respType.ToString();
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
        public string EndpointURL = "http://apiharness.ensygnia.net/OnescanCallback.aspx";
    }

    

    
}
