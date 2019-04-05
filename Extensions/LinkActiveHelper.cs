using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Auction.Extensions
{
    public static class LinkActiveHelper
    {
        public static IHtmlContent IsActive(this IHtmlHelper html,
                                          string control,
                                          string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            var returnActive = control == routeControl &&
                               action == routeAction;

            return new HtmlString(returnActive ? "active" : "");
        }
    }
}