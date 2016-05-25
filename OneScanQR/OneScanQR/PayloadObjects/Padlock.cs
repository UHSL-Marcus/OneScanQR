using Newtonsoft.Json;
using System.Web.Http;

namespace OneScanQR.PayloadObjects
{
    class Padlock
    {
        public string padlock;

        public Padlock(string data)
        {
            padlock = data;
        }

        public string GetJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
