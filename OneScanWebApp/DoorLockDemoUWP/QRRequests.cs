using CryptoLibUWP;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            HTTPResponse response = await getRequest.Request(url);
          
        }

        public static void doGetResult(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            string query = "mode=0&door_id=34";
            string hmac = HMAC.Hash(query, "56", HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanGetResult.ashx?" + query;

            
            int status = -1;
            while (status < 2)
            {
                byte[] reply;
                if (worker.CancellationPending) { status = 3; e.Cancel = true; }
                else if (Get.HTTPGetRequest(url, out reply))
                    int.TryParse(Encoding.UTF8.GetString(reply), out status);

                if (status == 1) worker.ReportProgress(1);
            }

            e.Result = status;
        }
    }
}
