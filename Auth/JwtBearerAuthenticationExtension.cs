using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auction.Api.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public static class JwtBearerAuthenticationExtension
    {
        /// <summary>
        /// 注册JWT Bearer认证服务的静态扩展方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="appSettings">JWT授权的配置项</param>
        public static void AddJwtBearerAuthentication(this IServiceCollection services, AuctionSettings appSettings)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    o.RequireHttpsMetadata = true;
                    o.SaveToken = true;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = appSettings.IssuerSigningKey,

                        ValidateIssuer = true,
                        ValidIssuer = appSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = appSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });
        }

        /// <summary>
        /// 登陆用户
        /// </summary>
        public static string GenerateJwtToken(AuctionSettings appSettings, Claim[] claims) // ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Issuer = appSettings.Issuer,
            //     Audience = appSettings.Audience,
            //     Subject = claimsIdentity,
            //     IssuedAt = DateTime.Now,
            //     NotBefore = DateTime.Now.AddSeconds(3),
            //     Expires = DateTime.UtcNow.AddDays(7),
            //     SigningCredentials = appSettings.SigningCredentials
            // };


            // var token = tokenHandler.CreateToken(tokenDescriptor);

            // 创建用户身份标识

            // 创建令牌
            var token = new JwtSecurityToken(
                issuer: appSettings.Issuer,
                audience: appSettings.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: appSettings.SigningCredentials
            );
            return tokenHandler.WriteToken(token);
        }
    }
}
