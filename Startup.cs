using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Auction.Entities;
using Auction.Data;
using Auction.Extensions.Builder;
using System.Text;
using Auction.HealthChecks;
using Swashbuckle.AspNetCore.Swagger;
using Auction.Api.Auth;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Auction.Data.AutoMapper;
using Newtonsoft.Json.Serialization;
using System;
using Microsoft.AspNetCore.Identity;
using Action.Services;
using Auction.Identity.Entities;

namespace Auction
{
    public class Startup
    {
        private IServiceCollection _services;
        // public Startup(IHostingEnvironment env)
        // {
        //     var builder = new ConfigurationBuilder()
        //         .SetBasePath(env.ContentRootPath)
        //         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

        //     builder.AddEnvironmentVariables();
        //     _configuration = builder.Build();
        // }
        // public IConfigurationRoot _configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCors(o =>
                o.AddPolicy("*",
                    builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials()
                ));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // options.CheckConsentNeeded = context => true;
                // options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            var auctionSettingsSection = _configuration.GetSection("AuctionSettings");
            services.Configure<AuctionSettings>(auctionSettingsSection);
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserService, UserService>();

            services.AddDbContext<AuctionDbContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
                options.UseLazyLoadingProxies();
                options.EnableSensitiveDataLogging();
            });


            services.AddJwtBearerAuthentication(auctionSettingsSection.Get<AuctionSettings>());

            services.AddMemoryCache();

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
                cfg.ValidateInlineMaps = false;
            });
            services.AddAutoMapper();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".PhoneCode.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddMvc(options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    })
                    .AddJsonOptions(options =>
                    {
                        // options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        // 开启大小写，大写开头字段名
                        // options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; // 忽略在对象图中找到的循环引用
                        // options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // c.IncludeXmlComments(xmlPath);
            });
            services.AddHealthChecks()
                .AddCheck<HomePageHealthCheck>("home_page_health_check")
                .AddCheck<ApiHealthCheck>("api_health_check");

            // https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/http-context?view=aspnetcore-2.2
            services.AddHttpContextAccessor();

            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LinkGenerator linkGenerator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // ListAllRegisteredServices(app, linkGenerator);
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(_configuration.GetSection("AuctionSettings:FilesRootDir").Value, "images")),
                RequestPath = "/images"
            });
            // app.UseStaticFiles(
            //     new StaticFileOptions
            //     {
            //         FileProvider = new PhysicalFileProvider(Path.Combine("D:\\net_project\\photos")),
            //         RequestPath = "/"
            //     }
            // );
            app.UseSession();
            app.UseCookiePolicy();
            app.UseAuthentication(); // useMvc 上面
            app.UseHttpContextAccessor();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "api",
                    template: "{area:exists}/{controller=Account}/{action=Login}/{id?}")
                ;
            });
        }

        private void ListAllRegisteredServices(IApplicationBuilder app, LinkGenerator linkGenerator)
        {
            var homePageLink = linkGenerator.GetPathByAction("Index", "Catalog");
            var loginLink = linkGenerator.GetPathByAction("SignIn", "Account");
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<a href=\"");
                sb.Append(homePageLink);
                sb.AppendLine("\">Return to site</a> | ");
                sb.Append("<a href=\"");
                sb.Append(loginLink);
                sb.AppendLine("\">Login to site</a>");
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
    }
}
