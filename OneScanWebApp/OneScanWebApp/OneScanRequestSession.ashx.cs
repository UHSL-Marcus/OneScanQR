using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneScanWebApp.Utils;
using OneScanWebApp.PayloadObjects;
using OneScanWebApp.Database;
using System.IO;
using System.Web.SessionState;
using OneScanWebApp.Database.Objects;
using Newtonsoft.Json;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for RequestOneScanSessionHandler
    /// </summary>
    public class OneScanRequestSessionHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            int mode;
            if (!int.TryParse(context.Request.QueryString["mode"], out mode))
                return;

            string doorID = context.Request.QueryString["door_id"];
            string hmac = context.Request.QueryString["data"];

            int QR_img;
            if (!int.TryParse(context.Request.QueryString["qr_img"], out QR_img) || doorID == null || hmac == null)
                return;

            string toHmac = "mode=" + mode + "&qr_img=" + QR_img + "&door_id=" + doorID;

            string secret = "";
            LoginTypes loginType;
            
            switch (mode)
            {
                case 0:
                    List<Door> doors;
                    if (!SQLControls.getEntryByColumn(doorID, "DoorID", out doors) || doors.Count > 1)
                        return;
                    secret = doors[0].DoorSecret;
                    loginType = LoginTypes.UserToken;
                    break;
                case 1:
                    string key = context.Request.QueryString["key"];
                    toHmac += "&key=" + key;
                    List<RegistrationToken> regtokns;
                    if (!SQLControls.getEntryByColumn(key, "Id", out regtokns) || regtokns.Count > 1)
                        return;
                    secret = regtokns[0].Secret;
                    loginType = LoginTypes.Register;
                    break;
                default: return;
            }

           
            if (!HMAC.ValidateHash(toHmac, secret, hmac))
                return;  

            BasePayload payload = new BasePayload();
            payload.SetLoginPayload(loginType, doorID);

            string QR, sessionID;
            if (OneScanRequests.GetQRData(JsonUtils.GetJson(payload), out QR, out sessionID))
            {
                string t;
                Global.OneScanSessions.TryRemove(doorID, out t);
                
                if (!Global.OneScanSessions.TryAdd(doorID, sessionID))
                    return;

                if (QR_img == 1)
                {
                    byte[] byteArray = new byte[0];
                    using (MemoryStream stream = new MemoryStream())
                    {
                        QRGen.GenerateQRCode(QR).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        stream.Close();

                        byteArray = stream.ToArray();
                    }

                    QR = Convert.ToBase64String(byteArray);
                    
                }

                context.Response.Write(QR);

                

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    class RequestResponse
    {
        public string SessionID;
        public string Data;
    }
}