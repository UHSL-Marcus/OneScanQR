using OneScanWebServer.Payload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneScanWebServer.Utils
{
    class PayloadFactory
    {
        public static string GetPayload(LoginTypes type)
        {
            BasePayload payload = new BasePayload();
            payload.SetLoginPayload(type, Guid.NewGuid().ToString());
            return payload.GetJson();
        }
    }
}
