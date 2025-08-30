using ERP.Context;
using ERP.Models;
using ERP.Repositories.Interfaces.Sales;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Repositories.Sales {
    public class SalesRepository : ISaleRepository {

        private readonly AppDbContext _context;

        public SalesRepository(AppDbContext context) {
            _context = context;
        }
        public async Task AddAsync(Sale sale) {
            sale.Total = sale.Items.Sum(i => i.Quantity * i.UnitPrice);
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id) {
            var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return false;

            _context.SaleItems.RemoveRange(sale.Items);
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync() {
            return await _context.Sales
          .Include(s => s.Client)
          .Include(s => s.Items)
              .ThenInclude(i => i.Product)
          .ToListAsync();
        }

        public async Task<Sale> GetByIdAsync(int id) {
            return await _context.Sales
           .Include(s => s.Client)
           .Include(s => s.Items)
               .ThenInclude(i => i.Product)
           .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> UpdateAsync(Sale sale) {
            var existingSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            if (existingSale == null) return null;

            // Ajusta estoque: devolve produtos removidos ou reduz se quantidade aumentou
            foreach (var oldItem in existingSale.Items) {
                var product = await _context.Products.FindAsync(oldItem.ProductId);
                var newItem = sale.Items.FirstOrDefault(i => i.ProductId == oldItem.ProductId);

                if (newItem == null) // item removido
                    product.Stock += oldItem.Quantity;
                else // item existente
                    product.Stock += oldItem.Quantity - newItem.Quantity;
            }

            // Atualiza os itens da venda
            existingSale.Items = sale.Items;
            existingSale.ClientId = sale.ClientId;
            existingSale.PaymentMethod = sale.PaymentMethod;

            // Recalcula total
            existingSale.Total = existingSale.Items.Sum(i => i.Quantity * i.UnitPrice);

            _context.Sales.Update(existingSale);
            await _context.SaveChangesAsync();

            return existingSale;
        }
    }
}
