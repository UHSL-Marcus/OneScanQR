using HtmlAgilityPack;
using OneScanQR.PayloadObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace OneScanQR.Utils
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
            headers.Add("Host", "apiharness.ensygnia.net");
            //headers.Add("Connection", "keep-alive");
            headers.Add("Cache-Control", "max-age=0");
            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            headers.Add("Origin", "http://apiharness.ensygnia.net");
            headers.Add("Upgrade-Insecure-Requests", "1");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            headers.Add("Content-Type", "application/x-www-form-urlencoded");
            headers.Add("DNT", "1");
            headers.Add("Referer", "http://apiharness.ensygnia.net/");
            headers.Add("Accept-Encoding", "gzip, deflate");
            headers.Add("Accept-Language", "en-US,en;q=0.8");
            try
            {
                string reply;
                if (HTTPRequest.HTTPPostRequest(baseUrl + "/padlock.aspx", "data=" + data, out reply, headers))
                {
                    var replyHtml = new HtmlDocument();
                    replyHtml.LoadHtml(reply);
                    HtmlNode padlockDiv = replyHtml.GetElementbyId("onescanPadlock_1");
                    sessionUrl = padlockDiv.GetAttributeValue("data-sessionurl", "");
                    pollTarget = padlockDiv.GetAttributeValue("data-polltarget", "");

                    if (!string.IsNullOrEmpty(sessionUrl) && !string.IsNullOrEmpty(pollTarget))
                        success = QRRequest(data, out QR);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }

            return success;
        }

        private bool QRRequest(string data, out Bitmap QR)
        {
            bool success = false;
            QR = null;

            NameValueCollection headers = new NameValueCollection();
            headers.Add("Host", "apiharness.ensygnia.net");
            //headers.Add("Connection", "keep-alive");
            headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            headers.Add("Origin", "http://apiharness.ensygnia.net");
            headers.Add("X-Requested-With", "XMLHttpRequest");
            headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
            headers.Add("Content-Type", "application/json; charset=UTF-8");
            headers.Add("DNT", "1");
            headers.Add("Referer", "http://apiharness.ensygnia.net/padlock.aspx");
            headers.Add("Accept-Encoding", "gzip, deflate");
            headers.Add("Accept-Language", "en-US,en;q=0.8");

            try {
                string padlock = (new Padlock(data)).GetJson();
                string reply;
                if (HTTPRequest.HTTPPostRequest(baseUrl + sessionUrl, padlock, out reply, headers))
                {
                    jsonReply = RecievedQRData.GetObject(reply);

                    if (jsonReply.Success)
                    {
                        sessionID = jsonReply.qrImageData.Session.SessionID;
                        QR = QRGen.GenerateQRCode(jsonReply.qrImageData.QRData);
                        success = true;
                    }
                }
            } catch (Exception e)
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
