using OneScanWebApp.Database;
using OneScanWebApp.Database.Objects;
using OneScanWebApp.PayloadObjects;
using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for OneScanAdminRequestSession
    /// </summary>
    public class OneScanAdminRequestSession : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int mode;
            if (!int.TryParse(context.Request.QueryString["mode"], out mode))
                throw new HttpException(400, "Query Incomplete (Invalid Mode)");

            string guid = context.Request.QueryString["guid"];
            string hmac = context.Request.QueryString["data"];

            int QR_img;
            if (!int.TryParse(context.Request.QueryString["qr_img"], out QR_img) || guid == null || hmac == null)
                throw new HttpException(400, "Query Incomplete");

            string toHmac = "mode=" + mode + "&qr_img=" + QR_img + "&guid=" + guid;

            string secret = "";
            
            LoginTypes loginType;

            switch (mode)
            {
                case 0:
                    secret = ConfigurationManager.AppSettings["AdminSecret"];
                    loginType = LoginTypes.UserToken;
                    break;
                case 1:
                    string key = context.Request.QueryString["key"];
                    toHmac += "&key=" + key;
                    List<RegistrationToken> regtokns;
                    if (!SQLControls.getEntryByColumn(key, "AuthKey", out regtokns) || regtokns.Count > 1)
                        throw new HttpException(400, "Query Incomplete (Invalid AuthKey)");
                    secret = regtokns[0].Secret;
                    loginType = LoginTypes.Register;
                    break;
                default: throw new HttpException(400, "Query Incomplete (Mode out of range)");
            }

            //if (!HMAC.ValidateHash(toHmac, secret, hmac))
            //throw new HttpException(403, "Unauthorised");

            BasePayload payload = new BasePayload();
            payload.SetLoginPayload(loginType, guid, Consts.URL_BASE + "OneScanAdminCallback.ashx");

            string QR, sessionID;
            if (OneScanRequests.GetQRData(JsonUtils.GetJson(payload), out QR, out sessionID))
            {
                string t;
                Global.OneScanAdminSessions.TryRemove(guid, out t);

                if (!Global.OneScanAdminSessions.TryAdd(guid, sessionID))
                    throw new HttpException(500, "Session error");

                if (QR_img == 1 || QR_img == 2)
                {

                    QRGen qrgen = new QRGen(QR, QRCoder.QRCodeGenerator.ECCLevel.H);

                    if (QR_img == 1)
                    {
                        context.Response.ContentType = "image/bmp";
                        byte[] qrArr = qrgen.getLSBOrderedPixels(256, 256);
                        context.Response.OutputStream.Write(qrArr, 0, qrArr.Length);
                    }


                    if (QR_img == 2)
                        context.Response.Write(Convert.ToBase64String(qrgen.get1BitBitmapByteArray(256, 256)));

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
}