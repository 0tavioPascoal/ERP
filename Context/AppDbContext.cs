using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Context {
    public class AppDbContext: DbContext {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleItem> SaleItems { get; set; }

    }
}
