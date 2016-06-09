using AdminWebApp.Database.Objects;
using AdminWebPortal.Database;
using AdminWebPortal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdminWebPortal.Views.Main.QRPages
{
    [DataContract]
    public class QRMethods
    {
        [DataMember(Name = "key")]
        private string key;
        [DataMember(Name = "secret")]
        private string secret;
        [DataMember(Name = "guid")]
        private string guid;
        [DataMember(Name = "sessionUrl")]
        string sessionUrl = "http://localhost:3469/OneScan@RequestSession.ashx?";
        [DataMember(Name = "resultUrl")]
        string resultUrl = "http://localhost:3469/OneScan@GetResult.ashx?";

        public QRMethods(string key, string guid, bool admin = false)
        {
            this.key = key;
            this.guid = guid;

            string replace = "";
            if (admin) replace = "Admin";
            sessionUrl = sessionUrl.Replace("@", replace);
            resultUrl = resultUrl.Replace("@", replace);
        }

        public QRMethods() { }

        public bool GetRegistrationQR(ref Image qrImg, string hmac)
        {

            bool success = false;

            qrImg.ImageUrl = "";

            string toHmac = "guid=" + guid + "&key=" + key;

            List<RegistrationToken> regtokns;
            if (SQLControls.getEntryByColumn(key, "AuthKey", out regtokns) || regtokns.Count > 1)
            {

                secret = regtokns[0].Secret;

                if (HMAC.ValidateHash(toHmac, secret, hmac))
                {
                    
                    string query = "mode=1&qr_img=1&guid=" + guid + "&key=" + key;
                    hmac = HMAC.Hash(query, secret);
                    query += "&data=" + hmac;

                    byte[] reply;
                    if (HTTPRequest.HTTPGetRequest(sessionUrl + query, out reply))
                    {
                        qrImg.ImageUrl = "data:image/bmp;base64," + System.Text.Encoding.Default.GetString(reply);
                        success = true;
                    }

                }
                
            }
            return success;
        }
        public string getPollUrl()
        {
            string query = "mode=1&guid=" + guid + "&key=" + key;
            string hmac = HMAC.Hash(query, secret);
            query += "&data=" + hmac;

            return resultUrl + query;
        }

        public string checkStatus()
        {
            string returnS = "";

            byte[] reply;
            if (HTTPRequest.HTTPGetRequest(getPollUrl(), out reply))
            {
                int status;
                if (int.TryParse(System.Text.Encoding.Default.GetString(reply), out status))
                {
                    if (status < 2)
                    {
                        returnS += "pollTimeout();";
                        if (status == 1)
                        {
                            // scanning
                        }
                    }
                    else if (status >= 2)
                        returnS += "RegistrationFinish();";

                }
            }
            return returnS;
        }

    }
}