﻿using HTTPRequestLib;
using OneScanWebApp.PayloadObjects;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;

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
                byte[] bytes;
                if(Post.HTTPPostRequest(ConfigurationManager.AppSettings["OnescanServerURL"], data, out bytes, headers))
                {
                    reply = System.Text.Encoding.Default.GetString(bytes);
                    success = true;
                }
                else
                {
                    ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(SendOnescanPayload) " + ExceptionHistory.lastException.ToString());
                }

                
            }
            catch (Exception e)
            {
                success = false;
                ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(SendOnescanPayload) " + e.ToString());
            }

            return success;
        }

        public static  bool GetQRData(string data, out string QR, out string sessionID)
        {
            bool success = false;
            QR = null;
            sessionID = null;

            try
            {
                string reply;
                if (SendOneScanPayload(data, out reply))
                {
                    RecievedQRData jsonReply = JsonUtils.GetObject<RecievedQRData>(reply);

                    if (jsonReply.Success)
                    {
                        QR = jsonReply.qrImageData.QRData;
                        sessionID = jsonReply.qrImageData.Session.SessionID;
                        success = true;
                    }
                }
            }
            catch (Exception e)
            {
                ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(GetQR) " + e.Message);
                success = false;
                //throw new HttpException(500, "(GetQRData) " + e.Message);
            }

            return success;
        }

    }
}
