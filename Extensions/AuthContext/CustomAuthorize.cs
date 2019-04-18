using Auction.Data;
using Auction.Extensions.CustomException;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Auction.Extensions.AuthContext
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        //private readonly string _someFilterParameter;
        /// <summary>
        /// 
        /// </summary>
        public CustomAuthorizeAttribute()
        {
            //_someFilterParameter = someFilterParameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                // context.Result = nreView(nameof(login));
                // context.Result = new ForbidResult();
                // context.Result = new StatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                // return;
                // throw new UnauthorizeException();
            }
            else
            {
                string[] roles = Roles.Split(",").Select(p => p.Trim()).ToArray();

                if (roles.Contains(ApplicationRole.Member))
                {
                    var DeadlineAtStr = user.Claims.Where(c => c.Type == "DeadlineAt").Select(c => c.Value);

                    if (DeadlineAtStr.Count() > 0)
                    {
                        var DeadlineAt = DateTime.ParseExact(DeadlineAtStr.First(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        if (DeadlineAt < DateTime.Now)
                        {
                            context.Result = new ForbidResult();
                            return;
                        }
                    }
                    //     var _context = context.HttpContext.RequestServices.GetService(typeof(AuctionDbContext));
                    //     // var cl = user.Claims.Select(c => c.Type == typeName).First().Value();
                }

            }

            // you can also use registered services
            //var someService = context.HttpContext.RequestServices.GetService<ISomeService>();

            //var isAuthorized = someService.IsUserAuthorized(user.Identity.Name, _someFilterParameter);
            //if (!isAuthorized)
            //{
            //    context.Result = new StatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            //    return;
            //}
        }
    }
}
