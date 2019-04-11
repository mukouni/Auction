using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Auction.Identity.Extensions
{
    public static class SignInManagerExtensions
    {

        public static async Task SignInUserAsync(this SignInManager<ApplicationUser> _signInManager, ApplicationUser user, bool isPersistent, IEnumerable<Claim> customClaims)
        {
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (customClaims != null && claimsPrincipal?.Identity is ClaimsIdentity claimsIdentity)
            {
                claimsIdentity.AddClaims(customClaims);
            }
            await _signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                claimsPrincipal,
                new AuthenticationProperties { IsPersistent = isPersistent });
        }
    }
}