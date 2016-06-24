using Newtonsoft.Json;

namespace OneScanWebApp.Utils
{
    public class JsonUtils
    {
        public static string GetJson(object ob)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.MissingMemberHandling = MissingMemberHandling.Ignore;
            return JsonConvert.SerializeObject(ob, settings);

        }

        public static TYPE GetObject<TYPE>(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<TYPE>(json, settings);
        }
    }
}