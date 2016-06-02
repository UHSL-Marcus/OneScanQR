using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace OneScanWebApp.Utils
{
    public delegate void HTTPAsyncCallback(string s);
    class RequestStruct
    {
        public WebRequest request;
        public WebResponse response;
        public Stream responseStream;
        public string responsedata;
        public HTTPAsyncCallback asyncCallback;

        public RequestStruct()
        {
            request = null;
            responseStream = null;
            response = null;
            asyncCallback = null;
            responsedata = "";
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

        public static bool HTTPPostRequest(string url, string data, out string reply, NameValueCollection headers = null, NameValueCollection parameters = null)
        {
            bool success = false;
            reply = "";

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

            try
            {
                //using (WebClient client = GetWebClient(headers, parameters))
                //success = DecodeReply(client.UploadString(url, data), client, ref reply);

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
                Console.WriteLine("HTTP Exception: " + e.ToString() + " (" + e.Message + ")");
                success = false;
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
        public static bool HTTPGetRequest(string url, out string reply, NameValueCollection headers = null, NameValueCollection parameters = null)
        {
            bool success = false;
            reply = "";

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
            try
            {
                //using (WebClient client = GetWebClient(headers, parameters))
                //success = DecodeReply(client.DownloadString(url), client, ref reply);
                reqSt = new RequestStruct();

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GetUrl(url, parameters));
                req.Method = "GET";

                AddHeaders(ref req, headers);

                reqSt.request = req;

                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("HTTP Exception: " + e.ToString() + " (" + e.Message + ")");
                success = false;
            }

            return success;
        }

        /*private static WebClient GetWebClient(NameValueCollection headers, NameValueCollection parameters)
        {
            WebClient client = new WebClient();

            

            if (headers != null)
            {
                if (headers.Count > 0)
                {
                    foreach (string header in headers.AllKeys)
                        client.Headers.Add(header, headers[header]);
                }
            }

            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    foreach (string param in parameters.AllKeys)
                        client.QueryString.Add(param, parameters[param]);
                }
            }

            return client;
        }*/

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

        private static bool DoRequest(ref RequestStruct requestStruct, ref string reply)
        {
            bool success = false;

            requestStruct.response = requestStruct.request.GetResponse();
            requestStruct.responseStream = requestStruct.response.GetResponseStream();
            if (DecodeResponseStream(ref requestStruct))
            {
                reply = requestStruct.responsedata;
                success = true;
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

            using (Stream responseStream = rs.responseStream)
            {
                string encoding = "";
                if (rs.response.Headers.TryGetValue("Content-Encoding", out encoding) && encoding.Equals("gzip"))
                {
                    using (GZipStream decompressStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    {
                        rs.responsedata = (new StreamReader(decompressStream)).ReadToEnd();
                        success = true;
                    }

                }
                else { rs.responsedata = (new StreamReader(responseStream)).ReadToEnd(); success = true; }
            }

            return success;
        }

        

        /*private static bool DecodeReply(object input, WebClient client,  ref string reply)
        {
            bool success = false;

            using (Stream responseStream = new MemoryStream())
            {
                StreamWriter w = new StreamWriter(responseStream);
                w.Write(input);
                w.Flush();
                //responseStream.Position = 0;

                if (client.ResponseHeaders["Content-Encoding"].Equals("gzip"))
                {
                    using (GZipStream decompressStream = new GZipStream(responseStream, CompressionMode.Decompress))
                    {
                        reply = (new StreamReader(decompressStream)).ReadToEnd();
                        success = true;
                    }

                }
                else { reply = (new StreamReader(responseStream)).ReadToEnd(); success = true; }
            }

            return success;
        }*/
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
    }
}
