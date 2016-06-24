using System;
using System.Collections.Specialized;

namespace OneScanWebApp.Utils
{
    public static class Extentions
    {
        public static bool TryGetValue(this NameValueCollection col, string key, out string value)
        {
            value = "";
            bool success = false;

            try
            {
                value = col[key];
                success = true;
            }
            catch (Exception) { };

            return success;
        }

    }
}