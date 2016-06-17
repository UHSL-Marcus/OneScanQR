using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

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