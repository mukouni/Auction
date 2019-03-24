using Auction.Data;
using Auction.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Data
{
    public class AuctionDbContextSeed
    {
        public static async Task SeedAsync(AuctionDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            if (roleManager.FindByNameAsync(ApplicationRole.Admin) != null)
            {
                // await Task.Run(() =>
                // {
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = ApplicationRole.Admin,
                        NormalizedName = ApplicationRole.Admin.ToUpper()
                    });
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = ApplicationRole.Developer,
                        NormalizedName = ApplicationRole.Developer.ToUpper()
                    });
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = ApplicationRole.Staff,
                        NormalizedName = ApplicationRole.Staff.ToUpper()
                    });
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = ApplicationRole.Guest,
                        NormalizedName = ApplicationRole.Guest.ToUpper()
                    });
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = ApplicationRole.Member,
                        NormalizedName = ApplicationRole.Guest.ToUpper()
                    });
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Id = Guid.NewGuid(),
                        Name = ApplicationRole.Other,
                        NormalizedName = ApplicationRole.Other.ToUpper()
                    });

                // });


                var defaultUser = new ApplicationUser
                {
                    RealName = "康元",
                    UserName = "15510455963",
                    PhoneNumber = "15510455963",
                    Email = "blue@ascendantcn.com"
                };

                var User1 = new ApplicationUser
                {
                    RealName = "dmzsz",
                    UserName = "18501255963",
                    PhoneNumber = "18501255963",
                    Email = "dmzsz@sina.com"
                };

                var User2 = new ApplicationUser
                {
                    RealName = "mukouni",
                    UserName = "15618308191",
                    PhoneNumber = "15618308191",
                    Email = "mukouni@gmail.com"
                };

                // await Task.Run(() =>
                // {
                    await userManager.CreateAsync(defaultUser, "111111");
                    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("15510455963"), ApplicationRole.Admin);

                    await userManager.CreateAsync(User1, "111111");
                    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("18501255963"), ApplicationRole.Staff);

                    await userManager.CreateAsync(User2, "111111");
                    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("15618308191"), ApplicationRole.Member);
                // });

            }
        }
    }
}
