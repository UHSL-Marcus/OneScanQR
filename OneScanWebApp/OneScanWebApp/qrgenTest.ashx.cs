using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for qrgenTest
    /// </summary>
    public class qrgenTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            QRGen qr = new QRGen("http://ensygnia.com/?pn0gENH/cWIC+AXqLEII5Q==", QRCoder.QRCodeGenerator.ECCLevel.H);

            byte[] full = qr.getBitmapArray(128, 128);
            byte[] pixels = qr.getBitmapArray(128, 128, false);

            Bitmap bitmap = qr.getBitmap(128, 128);

            //File.WriteAllBytes(@"c:\qrcodeBmp.bmp", full);
            //File.WriteAllBytes(@"c:\qrcodePixels", pixels);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (byte b in pixels)
            {
                sb.Append("0x" + b.ToString("X2") + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            //File.WriteAllText(@"c:\bmpHexArray.c", sb.ToString());

            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                Bitmap convBmp = BitmapConvert.CopyToBpp(bitmap, 1);
                convBmp.Save(stream, ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }

            context.Response.ContentType = "image/bmp";
            //context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
            context.Response.OutputStream.Write(full, 0, full.Length);
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