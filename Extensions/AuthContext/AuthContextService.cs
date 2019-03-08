using Auction.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using  Auction.Entities.Enums;

namespace Auction.Extensions.AuthContext
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthContextService
    {
        /// <summary>
        /// 被注入的httpContextAccessor实例
        /// </summary>
        private static IHttpContextAccessor _context;

        /// <summary>
        /// httpContextAccessor注入入口
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }

        /// <summary>
        /// 当前http context
        /// </summary>
        public static HttpContext Current => _context.HttpContext;

        /// <summary>
        /// 返回当前请求中的User信息
        /// </summary>
        public static AuthContextUser CurrentUser
        {
            get
            {
                var user = new AuthContextUser
                {
                    Guid = new Guid(Current.User.FindFirstValue(ClaimTypes.Sid)),
                    LoginName = Current.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    DisplayName = Current.User.FindFirstValue(ClaimTypes.Name),
                    Phone = Current.User.FindFirstValue(ClaimTypes.MobilePhone),
                    Email = Current.User.FindFirstValue(ClaimTypes.Email),
                    Role = Current.User.FindFirstValue(ClaimTypes.Role),
                    Avator = Current.User.FindFirstValue("avator")
                };

                return user;
            }
        }

        /// <summary>
        /// 是否已授权
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return Current.User.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public static bool IsAdministator
        {
            get
            {
                return Current.User.IsInRole(CommonEnum.UserRole.Admin.ToString());
            }
        }

        /// <summary>
        /// 是否是客人
        /// </summary>
        public static bool IsGuest
        {
            get
            {
                return Current.User.IsInRole(CommonEnum.UserRole.Guest.ToString());
            }
        }


        /// <summary>
        /// 是否是会员
        /// </summary>
        public static bool IsMember
        {
            get
            {
                return Current.User.IsInRole(CommonEnum.UserRole.Member.ToString());
            }
        }

        /// <summary>
        /// 是否是员工
        /// </summary>
        public static bool IsStaff
        {
            get
            {
                return Current.User.IsInRole(CommonEnum.UserRole.Staff.ToString());
            }
        }
    }
}
