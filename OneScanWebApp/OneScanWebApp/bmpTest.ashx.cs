using OneScanWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for bmpTest
    /// </summary>
    public class bmpTest : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ProcessRequest(HttpContext context)
        {


            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                Bitmap bmp = QRGen.GenerateQRCode("someinfotoQR");

                Bitmap eightbit = BitmapConvert.CopyToBpp(bmp, 8);

                eightbit.Save(stream, ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }
            context.Response.ContentType = "image/bmp";
            context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);

        }
    
        

    }
}