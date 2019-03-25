using System;
using Microsoft.AspNetCore.Http;

namespace Auction.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

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
    }
}