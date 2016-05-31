using OneScanWebServer.Payload;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneScanWebServer.Utils
{
    class OneScanRequests
    {
        string baseUrl = "http://apiharness.ensygnia.net";
        string sessionUrl;
        string pollTarget;
        string sessionID;
        RecievedQRData jsonReply;

        public bool padlockStart(string data, out Bitmap QR)
        {
            bool success = false;
            QR = null;

            NameValueCollection headers = new NameValueCollection();
            
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            headers.Add("Content-Type", "application/json");
            headers.Add("x-onescan-account", ConfigurationManager.AppSettings["AuthKey"]);
            string hmac = HMAC.Hash(data, ConfigurationManager.AppSettings["AuthSecret"]);
            headers.Add("x-onescan-signature", hmac);
            try
            {
                string reply;
                if (HTTPRequest.HTTPPostRequest(ConfigurationManager.AppSettings["OnescanServerURL"], data, out reply, headers))
                {
                    jsonReply = RecievedQRData.GetObject(reply);

                    if (jsonReply.Success)
                    {
                        sessionID = jsonReply.qrImageData.Session.SessionID;
                        QR = QRGen.GenerateQRCode(jsonReply.qrImageData.QRData);
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

        public delegate void padLockScannedCallback(bool success, string s);
        public bool padlockContinue(padLockScannedCallback final)
        {
            bool success = false;

            NameValueCollection headers = new NameValueCollection();
            headers.Add("Host", "liveservice.ensygnia.net");
            //headers.Add("Connection", "keep-alive");
            headers.Add("Accept", "*/*");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            headers.Add("DNT", "1");
            headers.Add("Referer", "http://apiharness.ensygnia.net/padlock.aspx");
            headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            headers.Add("Accept-Language", "en-US,en;q=0.8");

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("OnescanSessionID", sessionID);

            HTTPAsyncCallback callback = null; 
            callback = delegate(string reply)
            {
                RecievedStatusData data = RecievedStatusData.GetObject(reply);
                if (data.Status == 2)
                    final(true, data.RedirectURL);
                else
                {
                    Thread.Sleep(1000);
                    if (data.Status == 3 || !HTTPRequest.HTTPGetRequestAsync(pollTarget, callback, headers, parameters))
                        final(false, "");
                }
            };
           
            try
            {
                if (HTTPRequest.HTTPGetRequestAsync(pollTarget, callback, headers, parameters))
                {
                    success = true;
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
