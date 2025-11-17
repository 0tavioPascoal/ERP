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

        // Adiciona serviços
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            string conn = Environment.GetEnvironmentVariable("DB_CONNECTION");

            if (string.IsNullOrEmpty(conn))
                throw new InvalidOperationException("DB_CONNECTION não configurada.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(conn)
            );

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>( opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;

            });

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()  
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy)); 
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; 
                options.AccessDeniedPath = "";
            });

            FastReport.Utils.RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

            // Injeção de dependência
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // Repository
            services.AddScoped<ISaleRepository, SalesRepository>();

            // Service
            services.AddScoped<SaleService>();
            services.AddScoped<SalesGraphicService>();

            services.AddScoped<ReportService>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            // 👉 Forçando cultura "en-US" para que decimais usem ponto (.)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseFastReport();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
