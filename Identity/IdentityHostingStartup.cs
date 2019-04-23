using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Auction.Data;
using Auction.Identity.Services;
using Auction.Identity.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Auction.Identity.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using System.IdentityModel.Tokens.Jwt;
using Auction.Entities;

[assembly: HostingStartup(hostingStartupType: typeof(Auction.Identity.IdentityHostingStartup))]
namespace Auction.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<AuctionDbContext>(options =>
                {
                    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection"));
                    options.UseLazyLoadingProxies();
                    options.EnableSensitiveDataLogging();
                });

                services.AddIdentity<ApplicationUser, ApplicationRole>()
                        .AddEntityFrameworkStores<AuctionDbContext>()
                        // .AddUserManager<UserManager<ApplicationUser>>()
                        // .AddUserStore<ApplicationUser>()
                        .AddSignInManager()
                        .AddDefaultTokenProviders();

                // IdentityModelEventSource.ShowPII = true;

                // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

                // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                //         .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                //         {
                //             //认证失败，会自动跳转到这个地址
                //             options.LoginPath = "/account/login";
                //         });

                // services.Configure<CookiePolicyOptions>(options =>
                // {
                //     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //     //options.CheckConsentNeeded = context => true;
                //     options.MinimumSameSitePolicy = SameSiteMode.None;
                // });

                services.AddTransient<ISmsSender, SmsSender>();
                services.AddTransient<IEmailSender, EmailSender>();

                // services.AddAuthentication(o =>
                // {
                //     o.DefaultScheme = IdentityConstants.ApplicationScheme;
                //     o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                // })
                // .AddIdentityCookies(o => { });

                services.Configure<AuthMessageSenderOptions>(context.Configuration.GetSection("AuctionSettings"));

                services.Configure<IdentityOptions>(options =>
                {
                    // options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false; // 需要确认的电话号码进行登录。
                    // Password settings.
                    options.Password.RequireDigit = true;            // 需要介于 0-9 的密码。
                    options.Password.RequireLowercase = false;       // 要求密码中的小写字符。
                    options.Password.RequireUppercase = false;       // 需要大写字符的密码。
                    options.Password.RequireNonAlphanumeric = false; // 需要在密码中的非字母数字字符。
                    options.Password.RequiredUniqueChars = 0;        // 要求在密码中非重复字符数。
                    options.Password.RequiredLength = 6;             // 密码最小长度。

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5; // 用户被锁定之前允许的失败访问尝试次数
                    options.Lockout.AllowedForNewUsers = true;   // 确定是否新用户会被锁定。

                    // User settings.
                    options.User.AllowedUserNameCharacters = null;
                    // "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // 在用户名中允许的字符。
                    options.User.RequireUniqueEmail = false; // 要求每个用户必须拥有唯一的电子邮件。
                });

                services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.ForwardDefaultSelector = ctx =>
                    {
                        return ctx.Request.Path.StartsWithSegments("/api") ? JwtBearerDefaults.AuthenticationScheme : null;
                    };
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(12); //FromMinutes(5); 自动退出登陆的时间

                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.SlidingExpiration = true;

                    options.Cookie.Expiration = TimeSpan.FromDays(1);

                    options.Cookie = new CookieBuilder
                    {
                        IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                    };
                });
            });
        }
    }
}