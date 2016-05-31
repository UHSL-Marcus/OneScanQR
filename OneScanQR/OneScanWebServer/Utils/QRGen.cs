using QRCoder;
using System.Drawing;


namespace OneScanWebServer.Utils
{
    class QRGen
    {
        public static Bitmap GenerateQRCode(string info)
        {
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData qrData = qrGen.CreateQrCode(info, QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrData);
            return qrCode.GetGraphic(20);
        }
    }
}
