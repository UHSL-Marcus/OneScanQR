using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneScanQR.PayloadObjects
{
    class RecievedStatusData
    {
        public bool RedirectAsFormPost;
        public string RedirectURL;
        public string SessionID;
        public int? Status;

        public static RecievedStatusData GetObject(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<RecievedStatusData>(json, settings);
        }
    }
}
