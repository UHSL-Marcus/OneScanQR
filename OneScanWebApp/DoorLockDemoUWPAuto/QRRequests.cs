using CryptoLibUWP;
using HTTPRequestLibUWP;
using System;
using System.Threading.Tasks;

namespace DoorLockDemoUWPAuto
{
    class QRRequests
    {
        public async static Task<HTTPResponse> doGetQR()
        {
            string query = "mode=0&qr_img=2&door_id=" + Consts.DoorID;

            string hmac = HMAC.Hash(query, Consts.DoorSecret, HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanRequestSession.ashx?" + query;

            using (Get getRequest = new Get())
                return await getRequest.Request(url);
        }

        public async static Task<int> doGetResult(IProgress<int> progress)
        {

            string query = "mode=0&door_id=" + Consts.DoorID;
            string hmac = HMAC.Hash(query, Consts.DoorSecret, HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanGetResult.ashx?" + query;


            
            int status = -1;
            using (Get getRequest = new Get())
            {

                HTTPResponse response = new HTTPResponse();
                while (status < 2)
                {
                    response = await getRequest.Request(url);
                    if (int.TryParse(System.Text.Encoding.UTF8.GetString(response.bytes), out status))
                    {
                        if (status == 1) progress.Report(1);
                    }

                    await Task.Delay(500);

                }
            }

            return status;
            
        }
    }
}
