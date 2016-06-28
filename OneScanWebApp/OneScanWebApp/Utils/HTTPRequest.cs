using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace OneScanWebAppTT.Utils
{
    /*public delegate void HTTPAsyncCallback(byte[] ba);
    class RequestStruct
    {
        public WebRequest request;
        public WebResponse response;
        public Stream responseStream;
        public byte[] responsedata;
        public HTTPAsyncCallback asyncCallback;

        public RequestStruct()
        {
            request = null;
            responseStream = null;
            response = null;
            asyncCallback = null;
            responsedata = new byte[0];
        }

    }
    class HTTPRequest
    {
        public static bool HTTPPostRequestAsync(string url, string data, HTTPAsyncCallback callback, NameValueCollection headers = null, NameValueCollection parameters = null)
        {
            bool success = false;

            RequestStruct requestStruct;
            if (HTTPBuildPostRequest(url, data, out requestStruct, headers, parameters))
            {
                requestStruct.asyncCallback = callback;
                IAsyncResult r = requestStruct.request.BeginGetResponse(new AsyncCallback(RequestCallback), requestStruct);
                success = true;
            }

            return success;
        }

        public static bool HTTPPostRequest(string url, string data, out byte[] reply, NameValueCollection headers = null, NameValueCollection parameters = null)
        {
            bool success = false;
            reply = new byte[0];

            RequestStruct requestStruct;
            if (HTTPBuildPostRequest(url, data, out requestStruct, headers, parameters))
            {
                success = DoRequest(ref requestStruct, ref reply);
            }

            return success;
        }

        private static bool HTTPBuildPostRequest(string url, string data, out RequestStruct reqSt, NameValueCollection headers, NameValueCollection parameters)
        {
            bool success = false;
            reqSt = null;
            int count = 0;
            string messages = "";

            while (!success && count++ < 3)
            {
                try
                {

                    reqSt = new RequestStruct();

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GetUrl(url, parameters));
                    req.Method = "POST";
                    req.ContentLength = Encoding.UTF8.GetByteCount(data);

                    AddHeaders(ref req, headers);

                    StreamWriter stmw = new StreamWriter(req.GetRequestStream());
                    stmw.Write(data);
                    stmw.Flush();

                    reqSt.request = req;

                    success = true;

                }
                catch (Exception e)
                {
                    messages += e.ToString() + "\n\n";
                    if (count < 3)
                        Thread.Sleep(1000);
                    else
                    {
                        ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(BuildPost) " + messages);
                        success = false;
                        //throw new HttpException(500, "(HTTPBuildPostRequest) " + e.ToString()); // change to logging
                    }
                }
            }
            
            return success;
        }

        
        public static bool HTTPGetRequestAsync(string url, HTTPAsyncCallback callback, NameValueCollection headers = null, NameValueCollection parameters = null)
        {
            bool success = false;

            RequestStruct requestStruct;
            if (HTTPBuildGetRequest(url, out requestStruct, headers, parameters))
            {
                requestStruct.asyncCallback = callback;
                IAsyncResult r = requestStruct.request.BeginGetResponse(new AsyncCallback(RequestCallback), requestStruct);
                success = true;
            }

            return success;
        }
        public static bool HTTPGetRequest(string url, out byte[] reply, NameValueCollection headers = null, NameValueCollection parameters = null)
        {
            bool success = false;
            reply = new byte[0];

            RequestStruct requestStruct;
            if (HTTPBuildGetRequest(url, out requestStruct, headers, parameters))
            {
                success = DoRequest(ref requestStruct, ref reply);
            }

            return success;
        }

        private static bool HTTPBuildGetRequest(string url, out RequestStruct reqSt, NameValueCollection headers, NameValueCollection parameters)
        {

            bool success = false;
            reqSt = null;
            int count = 0;
            string messages = "";

            while (!success && count++ < 3)
            {
                try
                {
                    reqSt = new RequestStruct();

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GetUrl(url, parameters));
                    req.Method = "GET";

                    AddHeaders(ref req, headers);

                    reqSt.request = req;

                    success = true;
                }
                catch (Exception e)
                {
                    messages += e.ToString() + "\n\n";
                    if (count < 3)
                        Thread.Sleep(1000);
                    else
                    {
                        ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(BuildGet) " + messages);
                        success = false;
                        //throw new HttpException(500, "(HTTPBuildGetRequest) " + e.ToString()); // change to logging
                    }

                }
            }

            return success;
        }

        private static string GetUrl(string url, NameValueCollection parameters)
        {
            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    bool first = false;
                    foreach (string param in parameters.AllKeys)
                        url += (first ? "&" : "?") + param + "=" + parameters[param];
                }
            }

            return url;
        }


        private static void AddHeaders(ref HttpWebRequest req, NameValueCollection headers)
        {
            if (headers != null)
            {
                foreach (string header in headers.AllKeys)
                {
                    string val = headers[header];
                    switch (header)
                    {
                        case "Connection": if (val.Equals("keep-alive")) req.KeepAlive = true; break;
                        case "Host": req.Host = val; break;
                        case "Accept": req.Accept = val; break;
                        case "User-Agent": req.UserAgent = val; break;
                        case "Content-Type": req.ContentType = val; break;
                        case "Referer": req.Referer = val; break;
                        default: req.Headers.Add(header, val); break;
                    }
                }
            }
        }

        private static bool DoRequest(ref RequestStruct requestStruct, ref byte[] reply)
        {
            int count = 0;
            string messages = "";
            bool success = false;

            while (!success && count++ < 3)
            {
                try
                {
                    requestStruct.response = requestStruct.request.GetResponse();
                    requestStruct.responseStream = requestStruct.response.GetResponseStream();
                    if (DecodeResponseStream(ref requestStruct))
                    {
                        reply = requestStruct.responsedata;
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    messages += e.ToString() + "\n\n";
                    if (count < 3)
                        Thread.Sleep(1000);
                    else
                    {
                        ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(DoRequest) " + messages);
                        success = false;
                        //throw new HttpException(500, "(DoRequest) " + e.ToString()); // change to logging
                    }
                }
            }

            return success;
        }

        
        private static void RequestCallback(IAsyncResult ar)
        {
            RequestStruct requestStruct = (RequestStruct)ar.AsyncState;
            requestStruct.response = requestStruct.request.EndGetResponse(ar);
            requestStruct.responseStream = requestStruct.response.GetResponseStream();
            if (DecodeResponseStream(ref requestStruct))
            {
                requestStruct.asyncCallback(requestStruct.responsedata);
            }
        }

        private static bool DecodeResponseStream(ref RequestStruct rs)
        {
            bool success = false;
            try
            {

                using (Stream responseStream = rs.responseStream)
                {
                    string encoding = "";
                    if (rs.response.Headers.TryGetValue("Content-Encoding", out encoding) && encoding.Equals("gzip"))
                    {
                        using (GZipStream decompressStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            rs.responsedata = StreamToArray(decompressStream);
                            success = true;
                        }

                    }
                    else { rs.responsedata = StreamToArray(responseStream); success = true; }
                }
            }
            catch (Exception e)
            {
                ((Global)HttpContext.Current.ApplicationInstance).UpdateLog("(DecodeResponse) " + e.ToString());
                success = false;
                //throw new HttpException(500, "(DecodeResponseStream) " + e.ToString()); // change to logging
                throw e;
            }

            return success;
        }

        private static byte[] StreamToArray(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    public static class WebHeadersCollectionExtention
    {
        public static bool TryGetValue(this WebHeaderCollection headers, string key, out string val)
        {
            bool success = false;
            val = "";

            if (headers.AllKeys.Contains(key))
            {
                val = headers[key];
                success = true;
            }

            return success;
        }
    }*/
}
