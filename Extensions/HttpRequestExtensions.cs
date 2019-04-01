using System;
using Microsoft.AspNetCore.Http;

namespace Auction.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";
        private const string RequestGet = "GET";
        private const string RequestPost = "POST";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                var xhr = request.Headers[RequestedWithHeader];
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }

        public static bool IsAjaxGetRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                var xhr = request.Headers[RequestedWithHeader];
                return request.Method == RequestGet &&
                        request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }
        public static bool IsAjaxPostRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                var xhr = request.Headers[RequestedWithHeader];
                return request.Method == RequestPost &&
                        request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }
    }
}