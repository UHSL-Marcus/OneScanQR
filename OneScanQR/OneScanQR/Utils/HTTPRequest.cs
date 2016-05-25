using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace OneScanQR.Utils
{
    class HTTPRequest
    {
        public static bool HTTPJsonRequest(string json, out string reply)
        {
            bool success = false;
            reply = "temp";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://apiharness.ensygnia.net/RequestOnescanSession.aspx");
                req.Method = "POST";
                req.Host = "apiharness.ensygnia.net";
                req.KeepAlive = true;
                req.Accept = " application/json, text/javascript, */*; q=0.01";
                req.Referer = "http://apiharness.ensygnia.net/padlock.aspx";
                req.ContentType = "application/json; charset=UTF-8";
                req.ContentLength = Encoding.UTF8.GetByteCount(json);

                req.Headers.Add("Origin", "http://apiharness.ensygnia.net");
                req.Headers.Add("X-Requested-With", "XMLHttpRequest");
                req.Headers.Add("DNT", "1");
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "en-US,en;q=0.8");

                StreamWriter stmw = new StreamWriter(req.GetRequestStream());
                stmw.Write(json);
                stmw.Flush();

                WebResponse resp = req.GetResponse();
                string[] header = resp.Headers.AllKeys;
                StreamReader r;

                if (resp.Headers["Content-Encoding"].Equals("gzip"))
                {
                    using (Stream responseStream = resp.GetResponseStream())
                    {
                        using (GZipStream decompressStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            reply = (new StreamReader(decompressStream)).ReadToEnd();
                            success = true;
                        }
                    }
                }
                else { reply = (new StreamReader(resp.GetResponseStream())).ReadToEnd(); success = true; }

            }
            catch (Exception e)
            {
                Console.WriteLine("HTTP Exception: " + e.ToString() + " (" + e.Message + ")");
                success = false;
            }
            return success;
        }
    }
}
