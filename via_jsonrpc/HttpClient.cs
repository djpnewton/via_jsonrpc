using System;
using System.Net;
using System.Net.Http;

namespace via_jsonrpc {
    public class HttpClient {
        private string url;
        private WebClient w = new WebClient ();

        public HttpClient (string url) {
            this.url = url;
        }

        public string PostJson (string json) {
            WebRequest r = WebRequest.Create (url);
            r.ContentType = "application/json-rpc";
            r.Method = "POST";
            w.Headers.Add ("Content-Type", "application/json");
            return w.UploadString (url, "POST", json);
        }

    }
}