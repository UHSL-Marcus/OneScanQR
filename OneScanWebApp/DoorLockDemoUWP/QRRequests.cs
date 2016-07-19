using CryptoLibUWP;
using System.Threading;
using System.Threading.Tasks;
using HTTPRequestLibUWP;
using System;

namespace DoorLockDemoUWP
{
    class QRRequests
    {
        public async static Task<HTTPResponse> doGetQR(CancellationToken ct)
        {
            string query = "mode=0&qr_img=2&door_id=34";

            string hmac = HMAC.Hash(query, "56", HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanRequestSession.ashx?" + query;

            using (Get getRequest = new Get())
                return await getRequest.Request(url, ct);
        }

        public async static Task<int> doGetResult(CancellationToken ct, IProgress<int> progress)
        {

            string query = "mode=0&door_id=34";
            string hmac = HMAC.Hash(query, "56", HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanGetResult.ashx?" + query;


            
            int status = -1;
            using (Get getRequest = new Get())
            {

                HTTPResponse response = new HTTPResponse();
                while (status < 2)
                {
                    ct.ThrowIfCancellationRequested();

                    response = await getRequest.Request(url, ct);
                    if (int.TryParse(System.Text.Encoding.UTF8.GetString(response.bytes), out status))
                    {
                        if (status == 1) progress.Report(1);
                    }

                }
            }

            return status;
            
        }
    }
}
