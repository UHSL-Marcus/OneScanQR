﻿using OneScanWebApp.Database;
using OneScanWebApp.Database.Objects;
using OneScanWebApp.PayloadObjects;
using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for OneScanAdminRequestSession
    /// </summary>
    public class OneScanAdminRequestSession : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int mode;
            if (!int.TryParse(context.Request.QueryString["mode"], out mode))
                return;

            string guid = context.Request.QueryString["guid"];
            string hmac = context.Request.QueryString["data"];

            int QR_img;
            if (!int.TryParse(context.Request.QueryString["qr_img"], out QR_img) || guid == null || hmac == null)
                return;

            string toHmac = "mode=" + mode + "&qr_img=" + QR_img + "&guid=" + guid;

            string secret = "";
            
            LoginTypes loginType;
            AdminSessionData sData = new AdminSessionData();
            sData.guid = guid;

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
                        return;
                    secret = regtokns[0].Secret;
                    loginType = LoginTypes.Register;
                    sData.regkey = key;
                    break;
                default: return;
            }

            if (!HMAC.ValidateHash(toHmac, secret, hmac))
                return;

            BasePayload payload = new BasePayload();
            payload.SetLoginPayload(loginType, JsonUtils.GetJson(sData), "http://mmtsnap.mmt.herts.ac.uk/onescan/OneScanAdminCallback.ashx");

            string QR, sessionID;
            if (OneScanRequests.GetQRData(JsonUtils.GetJson(payload), out QR, out sessionID))
            {
                string t;
                Global.OneScanAdminSessions.TryRemove(guid, out t);

                if (!Global.OneScanAdminSessions.TryAdd(guid, sessionID))
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

    class AdminSessionData
    {
        public string guid;
        public string regkey;
    }
}