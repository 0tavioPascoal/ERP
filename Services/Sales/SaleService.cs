using ERP.Context;
using ERP.Models;
using ERP.Repositories.Interfaces.Sales;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services.Sales {
    public class SaleService {

        private readonly ISaleRepository _saleRepository;
        private readonly AppDbContext _context;

        public SaleService(ISaleRepository saleRepository, AppDbContext context) {
            _saleRepository = saleRepository;
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync() {
            return await _context.Sales
        .Include(s => s.Client)
        .Include(s => s.Items)
            .ThenInclude(i => i.Product)
        .ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(int id) {
            return await _saleRepository.GetByIdAsync(id);
        }

        public async Task<(bool Success, string? ErrorMessage)> AddAsync(Sale sale) {
            // 1️⃣ Verifica se o cliente existe
            var clientExists = await _context.Clients.AnyAsync(c => c.Id == sale.ClientId);
            if (!clientExists)
                return (false, "Cliente não encontrado.");

            // 2️⃣ Verifica produtos e estoque
            foreach (var item in sale.Items) {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return (false, $"Produto com ID {item.ProductId} não encontrado.");
                if (product.Stock < item.Quantity)
                    return (false, $"Estoque insuficiente para o produto {product.Name}.");

                item.UnitPrice = product.Price;
            }

            // 3️⃣ Calcula total da venda
            sale.Total = sale.Items.Sum(i => i.Quantity * i.UnitPrice);

            // 4️⃣ Atualiza estoque
            foreach (var item in sale.Items) {
                var product = await _context.Products.FindAsync(item.ProductId);
                product.Stock -= item.Quantity;
            }

            // 5️⃣ Adiciona a venda
            await _saleRepository.AddAsync(sale);

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(Sale sale) {
            var existingSale = await _saleRepository.GetByIdAsync(sale.Id);
            if (existingSale == null)
                return (false, "Venda não encontrada.");

            // Aqui você pode implementar lógica de atualização de estoque se necessário

            await _saleRepository.UpdateAsync(sale);
            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int id) {
            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                return (false, "Venda não encontrada.");

            // Reverte o estoque
            foreach (var item in sale.Items) {
                var product = await _context.Products.FindAsync(item.ProductId);
                product.Stock += item.Quantity;
            }

            var result = await _saleRepository.DeleteAsync(id);
            if (!result)
                return (false, "Erro ao deletar a venda.");

            return (true, null);
        }


    }
}
