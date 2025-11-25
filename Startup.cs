using dotenv.net;
using ERP.Context;
using ERP.Repositories.Clients;
using ERP.Repositories.Interfaces.Clients;
using ERP.Repositories.Interfaces.Products;
using ERP.Repositories.Interfaces.Sales;
using ERP.Repositories.Products;
using ERP.Repositories.Sales;
using ERP.Services.Graphic;
using ERP.Services.Reports;
using ERP.Services.Sales;
using FastReport.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;

namespace ERP {
    public class Startup {
        public Startup(IConfiguration configuration) {
            DotEnv.Load();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews(options => {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            string conn = Environment.GetEnvironmentVariable("DB_CONNECTION");

            if (string.IsNullOrEmpty(conn))
                throw new InvalidOperationException("DB_CONNECTION não configurada.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conn)
            );

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None; 
            });

            FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

   
            services.AddScoped<ISaleRepository, SalesRepository>();

 
            services.AddScoped<SaleService>();
            services.AddScoped<SalesGraphicService>();

            services.AddScoped<ReportService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

           

            app.UseStaticFiles();

            app.UseFastReport();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
