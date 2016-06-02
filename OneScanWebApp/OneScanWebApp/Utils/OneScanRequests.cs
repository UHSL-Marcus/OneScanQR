using OneScanWebApp.PayloadObjects;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Threading;

namespace OneScanWebApp.Utils
{
    class OneScanRequests
    {
        public static bool SendOneScanPayload(string data, out string reply)
        {
            bool success = false;
            reply = "";

            NameValueCollection headers = new NameValueCollection();

            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            headers.Add("Content-Type", "application/json");
            headers.Add("x-onescan-account", ConfigurationManager.AppSettings["AuthKey"]);
            string hmac = HMAC.Hash(data, ConfigurationManager.AppSettings["AuthSecret"]);
            headers.Add("x-onescan-signature", hmac);
            try
            {
                success = HTTPRequest.HTTPPostRequest(ConfigurationManager.AppSettings["OnescanServerURL"], data, out reply, headers);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }

            return success;
        }

        public static bool GetQRData(string data, out string QR)
        {
            bool success = false;
            QR = null;

            try
            {
                string reply;
                if (SendOneScanPayload(data, out reply))
                {
                    RecievedQRData jsonReply = JsonUtils.GetObject<RecievedQRData>(reply);

                    if (jsonReply.Success)
                    {
                        QR = jsonReply.qrImageData.QRData;
                        success = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }

            return success;
        }

    }
}
