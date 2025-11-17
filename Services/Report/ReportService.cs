using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Reports {
    public class ReportService {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportService(AppDbContext context, IWebHostEnvironment env) {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<Sale>> GetSaleReport() {
            var sales = await _context.Sales.ToListAsync();

            if (sales is null)
                return default(IEnumerable<Sale>);

            return sales;
        }

        public async Task<IEnumerable<SaleItem>> GetSaleItemsReport() {
            var salesI = await _context.SaleItems.ToListAsync();

            if (salesI is null)
                return default(IEnumerable<SaleItem>);

            return salesI;
        }

        public async Task<IEnumerable<Product>> GetProductReport() {
            var products = await _context.Products.ToListAsync();

            if (products is null)
                return default(IEnumerable<Product>);

            return products;
        }

        public async Task<IEnumerable<Client>> GetClientsReport() {
            var clients = await _context.Clients.ToListAsync();

            if (clients is null)
                return default(IEnumerable<Client>);

            return clients;
        }

    }
}
