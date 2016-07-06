using CryptoLib;
using HTTPRequestLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorLockDemo
{
    class QRRequests
    {
        public static void doGetQR(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string query = "mode=0&qr_img=2&door_id=" + Properties.Settings.Default.DoorID;

            string hmac = HMAC.Hash(query, Properties.Settings.Default.DoorSecret, HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanRequestSession.ashx?" + query;

            bool complete = false;
            HTTPAsyncCallback callback = delegate (bool success, byte[] reply)
            {
                if (worker.CancellationPending) e.Cancel = true;
                e.Result = Tuple.Create(success, reply);
                complete = true;
            };

            Get.HTTPGetRequestAsync(url, callback);

            while (!complete && !worker.CancellationPending){ }

        }

        public static void doGetResult(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            
            string query = "mode=0&door_id=" + Properties.Settings.Default.DoorID;
            string hmac = HMAC.Hash(query, Properties.Settings.Default.DoorSecret, HMAC.HmacAlgorithms.SHA1);
            query += "&data=" + hmac;

            string url = Consts.URL_BASE + "OneScanGetResult.ashx?" + query;

            
            int status = -1;
            while (status < 2)
            {
                byte[] reply;
                if (worker.CancellationPending) { status = 3; e.Cancel = true; }
                else if (Get.HTTPGetRequest(url, out reply))
                    int.TryParse(Encoding.Default.GetString(reply), out status);

                if (status == 1) worker.ReportProgress(1);

                Stopwatch sw = new Stopwatch();
                sw.Start();
 //               while (sw.ElapsedMilliseconds < 500L && !worker.CancellationPending) { }               
                sw.Stop();
            }

            e.Result = status;
        }
    }
}
