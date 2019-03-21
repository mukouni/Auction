using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Auction.Data;
using Microsoft.AspNetCore.Identity;
using Auction.Identity.Entities;

namespace Auction
{
    public class Program
    {
        // public static void Main(string[] args)
        // {
        //     CreateWebHostBuilder(args).Build().Run();
        // }

        // public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseKestrel(c =>
        //         {
        //             // c.AddServerHeader = false;
        //         })
        //         .UseContentRoot(Directory.GetCurrentDirectory())
        //         // .UseIISIntegration()
        //         .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                // var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                try
                {
                    var auctionDBContext = services.GetRequiredService<AuctionDbContext>();
                    AuctionDbContextSeed.SeedAsync(auctionDBContext, userManager, roleManager).Wait();
                }
                catch (Exception ex)
                {
                    // var logger = loggerFactory.CreateLogger<Program>();
                    // logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
            })
            .UseStartup<Startup>();
    }
}
