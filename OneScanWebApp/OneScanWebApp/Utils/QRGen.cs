using QRCoder;
using System.Drawing;


namespace OneScanWebApp.Utils
{
    class QRGen
    {
        public static Bitmap GenerateQRCode(string info)
        {
            QRCodeGenerator qrGen = new QRCodeGenerator();
            QRCodeData qrData = qrGen.CreateQrCode(info, QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrData);
            return qrCode.GetGraphic(4);
        }
    }
}
