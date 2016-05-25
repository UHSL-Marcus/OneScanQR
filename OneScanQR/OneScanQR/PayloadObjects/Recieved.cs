using Newtonsoft.Json;

namespace OneScanQR.PayloadObjects
{
    class Recieved
    {
        public string Version;
        public string[] Errors;
        public int? ErrorCount;
        public RecievedQRImageData qrImageData = new RecievedQRImageData();
        public bool PlayMode;
        public bool Success;

        public static Recieved GetObject(string json)
        {
            return JsonConvert.DeserializeObject<Recieved>(json);
        }
        
    }

    class RecievedQRImageData
    {
        public int? Protocol;
        public QRImageDataSession Session = new QRImageDataSession();
        public string ImageBytes;
        public string QRData;

    }

    class QRImageDataSession
    {
        public string SessionID;
        public int Status;
        public bool RedirectAsFormPost;
    }
}
