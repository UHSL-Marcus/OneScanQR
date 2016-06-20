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
using System.Drawing;
using System.Text;

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
                throw new HttpException(400, "Query Incomplete");

            string doorID = context.Request.QueryString["door_id"];
            string guid = context.Request.QueryString["guid"];
            string hmac = context.Request.QueryString["data"];

            int QR_img;
            if (!int.TryParse(context.Request.QueryString["qr_img"], out QR_img) || (mode == 1 && guid == null) || (mode == 0 && doorID == null) || hmac == null)
                throw new HttpException(400, "Query Incomplete");

            string toHmac = "mode=" + mode + "&qr_img=" + QR_img;

            string secret = "";
            LoginTypes loginType;
            SessionData sData = new SessionData();
            sData.doorID = doorID;
            
            switch (mode)
            {
                case 0:
                    toHmac += "&door_id = " + doorID;
                    List<Door> doors;
                    if (!SQLControls.getEntryByColumn(doorID, "DoorID", out doors) || doors.Count > 1)
                        throw new HttpException(400, "Query Incomplete");
                    secret = doors[0].DoorSecret;
                    loginType = LoginTypes.UserToken;
                    break;
                case 1:
                    string key = context.Request.QueryString["key"];
                    toHmac += "&guid=" + guid + "&key=" + key;
                    List<RegistrationToken> regtokns;
                    if (!SQLControls.getEntryByColumn(key, "AuthKey", out regtokns) || regtokns.Count > 1)
                        throw new HttpException(400, "Query Incomplete");
                    secret = regtokns[0].Secret;
                    loginType = LoginTypes.Register;
                    sData.regkey = key;
                    break;
                default: throw new HttpException(400, "Query Incomplete");
            }


            //if (!HMAC.ValidateHash(toHmac, secret, hmac))
            //throw new HttpException(403, "Unauthorised");

            BasePayload payload = new BasePayload();
            payload.SetLoginPayload(loginType, JsonUtils.GetJson(sData), Consts.URL_BASE + "OneScanCallback.ashx");

            string QR, sessionID;
            if (OneScanRequests.GetQRData(JsonUtils.GetJson(payload), out QR, out sessionID))
            {
                string sessionKey = doorID;
                if (sData.regkey != null)
                    sessionKey = sData.regkey;

                string t;
                Global.OneScanSessions.TryRemove(sessionKey, out t);

                if (!Global.OneScanSessions.TryAdd(sessionKey, sessionID))
                    throw new HttpException(500, "Session error");

                if (QR_img == 1 || QR_img == 2 || QR_img == 3)
                {
                    QRGen qrgen = new QRGen(QR, QRCoder.QRCodeGenerator.ECCLevel.H);

                    if (QR_img == 1 || QR_img == 3)
                    {
                        //context.Response.ContentType = "image/bmp";
                        byte[] qrArr = qrgen.getBitmapArray(128, 128, false);
                        byte[] qrArr8 = qrgen.get8BitBitmapArray(128, 128, false);

                        if (QR_img == 1)
                            context.Response.OutputStream.Write(qrArr8, 0, qrArr.Length);

                        if (QR_img == 3)
                            context.Response.Write(Encoding.UTF8.GetString(qrArr));

                        StringBuilder sb = new StringBuilder();
                        foreach (byte b in qrArr8)
                            sb.Append("0x" + b.ToString("X2") + ",");

                        string hexString = sb.ToString();

                        File.WriteAllText("c:/qrBytes.c", hexString);


                    }


                    if (QR_img == 2)
                        context.Response.Write(Convert.ToBase64String(qrgen.getBitmapArray(256, 256)));

                    

                }
                else context.Response.Write(QR);

            }
            else throw new Exception("Requesting QR from Onescan failed. errorID: '" + context.Session[Consts.ERROR_ID] + "'");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    class SessionData
    {
        public string doorID;
        public string regkey;
    }
}