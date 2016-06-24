using OneScanWebApp.Utils;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace OneScanWebApp
{
    /// <summary>
    /// Summary description for bytearraytest
    /// </summary>
    public class bytearraytest : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            QRGen qrG = new QRGen("http://ensygnia.com/?0yaou8WurWUJecjgyslzFQ==", QRCoder.QRCodeGenerator.ECCLevel.H);
            byte[] qr = qrG.getLSBOrderedPixels(128, 128);

            File.WriteAllBytes("c:/qrhexBytes", qr);

            StringBuilder sb = new StringBuilder();
            int count = 0;
            sb.Append("{");
            foreach (byte b in qr)
            {
                bool final = ++count % 16 == 0;
                sb.Append("0x" + b.ToString("X2"));
                if (!final) sb.Append(",");

                if (final)
                {
                    sb.Append("}");
                    if (count % (128 * 16) != 0)
                        sb.Append("\n{");
                }
               
            }

            string qrhexLines = sb.ToString();
            File.WriteAllText("c:/qrHexLines.txt", qrhexLines);

            sb = new StringBuilder();
            sb.Append("{");
            foreach (byte b in qr)
            {
                sb.Append("0x" + b.ToString("X2") + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            string qrhex = sb.ToString();
            File.WriteAllText("c:/qrHex.txt", qrhex);

            byte[] stripes = new byte[128 * 16];
            bool white = true;
            int rowCount = 0;
            for(int i = 0; i < stripes.Length; i++)
            {
                if (white) stripes[i] = 0xff;
                else stripes[i] = 0x00;

                if (++rowCount % (8*16) == 0)
                    white = !white;
            }
           
            sb = new StringBuilder();
            count = 0;
            BitArray ba = new BitArray(qr);
            foreach (bool b in ba)
            {
                bool final = ++count % 128 == 0;
                if (b) sb.Append("1");
                else sb.Append("0");

                if (final)
                    sb.Append("\n");

            }

            sb = new StringBuilder();
            count = 0;
            foreach (byte b in qr)
            {
                BitArray bits = new BitArray(new byte[] { b });
                bool final = ++count % 16 == 0;
                foreach(bool bit in bits)
                {
                    if (bit) sb.Append("1");
                    else sb.Append("0");
                }
                if (final)
                    sb.Append("\n");

            }

            string binaryDrawing = sb.ToString();

            context.Response.ContentType = "text/plain";
            context.Response.Write(binaryDrawing);

            //context.Response.ContentType = "image/bmp";

            byte[] bmp = qrG.get1BitBitmapByteArray(128, 128);

            //context.Response.OutputStream.Write(bmp, 0, bmp.Length);
            
            
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