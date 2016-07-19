using CryptoLibUWP;
using System.Threading;
using System.Threading.Tasks;
using HTTPRequestLibUWP;

namespace DoorLockDemoUWP
{
    class QRRequests
    {
        public async static Task doGetQR(CancellationToken ct)
        {
          
            string query = "mode=0&qr_img=2&door_id=34";

            string hmac = HMAC.Hash(query, "56", HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanRequestSession.ashx?" + query;

            Get getRequest = new Get();
            //HTTPResponse response = await getRequest.Request(url, ct);
          
        }

        public async static Task doGetResult(CancellationToken ct)
        {
            
            string query = "mode=0&door_id=34";
            string hmac = HMAC.Hash(query, "56", HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanGetResult.ashx?" + query;

            
            int status = -1;
            while (status < 2 && !ct.IsCancellationRequested)
            {

                Get getRequest = new Get();
                //HTTPResponse response = await getRequest.Request(url, ct);
                
            }

            
        }
    }
}
