using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneScanWebApp.Utils;
using OneScanWebApp.PayloadObjects;
using System.IO;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for RequestOneScanSessionHandler
    /// </summary>
    public class RequestOneScanSessionHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int QR_img;
            string doorID = context.Request.QueryString["door_id"];

            if (!int.TryParse(context.Request.QueryString["qr_img"], out QR_img) || doorID == null)
                return;

            BasePayload payload = new BasePayload();

            string id = (string)context.Session[Consts.GLOBAL_STATUS_ID];
            SessionData sd = new SessionData();
            sd.doorID = doorID;
            sd.sessionID = id;

            payload.SetLoginPayload(LoginTypes.UserToken, JsonUtils.GetJson(sd));

            string QR;
            if (OneScanRequests.GetQRData(JsonUtils.GetJson(payload), out QR))
            {
                if (QR_img == 1)
                {
                    byte[] byteArray = new byte[0];
                    using (MemoryStream stream = new MemoryStream())
                    {
                        QRGen.GenerateQRCode(QR).Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        stream.Close();

                        byteArray = stream.ToArray();
                        
                        context.Response.ContentType = "image/bmp";
                        context.Response.OutputStream.Write(byteArray, 0, byteArray.Count());
                    }
                    
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(QR);
                }

                
                if (Global.OneScanSessionStatus.Keys.Contains(id))
                    Global.OneScanSessionStatus[id] = "1";
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}