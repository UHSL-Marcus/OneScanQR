using Newtonsoft.Json;

namespace OneScanQR.PayloadObjects
{
    class RecievedQRData
    {
        public string Version;
        public string[] Errors;
        public int? ErrorCount;
        public RecievedQRImageData qrImageData = new RecievedQRImageData();
        public bool PlayMode;
        public bool Success;

        public static RecievedQRData GetObject(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.DeserializeObject<RecievedQRData>(json, settings);
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
