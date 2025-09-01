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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace ERP {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Adiciona serviços
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();

            // Conexão com o banco
            services.AddEntityFrameworkSqlServer().AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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

        // Configura o pipeline
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
