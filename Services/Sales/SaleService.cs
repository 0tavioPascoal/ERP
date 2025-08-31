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

        // Classe de resultado mais rica
        public record ServiceResult(bool Success, string? ErrorMessage = null, Sale? Data = null);

        public async Task<IEnumerable<Sale>> GetAllAsync() {
            return await _saleRepository.GetAllAsync();
        }

        public async Task<Sale?> GetByIdAsync(int id) {
            return await _saleRepository.GetByIdAsync(id);
        }

        public async Task<ServiceResult> AddAsync(Sale sale) {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            // 1️⃣ Verifica se o cliente existe
            var clientExists = await _context.Clients.AnyAsync(c => c.Id == sale.ClientId);
            if (!clientExists)
                return new ServiceResult(false, "Cliente não encontrado.");

            // 2️⃣ Verifica produtos e estoque (já atualiza estoque aqui)
            foreach (var item in sale.Items) {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return new ServiceResult(false, $"Produto com ID {item.ProductId} não encontrado.");
                if (product.Stock < item.Quantity)
                    return new ServiceResult(false, $"Estoque insuficiente para o produto {product.Name}.");

                item.UnitPrice = product.Price;
                product.Stock -= item.Quantity;
            }

            // 3️⃣ Calcula total
            sale.Total = sale.Items.Sum(i => i.Quantity * i.UnitPrice);

            // 4️⃣ Persiste a venda
            await _saleRepository.AddAsync(sale);
            await transaction.CommitAsync();

            return new ServiceResult(true, null, sale);
        }

        public async Task<ServiceResult> UpdateAsync(Sale sale) {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var existingSale = await _saleRepository.GetByIdAsync(sale.Id);
            if (existingSale == null)
                return new ServiceResult(false, "Venda não encontrada.");

            // 1️⃣ Reverte estoque dos itens antigos
            foreach (var oldItem in existingSale.Items) {
                var product = await _context.Products.FindAsync(oldItem.ProductId);
                if (product != null)
                    product.Stock += oldItem.Quantity;
            }

            // 2️⃣ Aplica novos itens (validando novamente)
            foreach (var newItem in sale.Items) {
                var product = await _context.Products.FindAsync(newItem.ProductId);
                if (product == null)
                    return new ServiceResult(false, $"Produto com ID {newItem.ProductId} não encontrado.");
                if (product.Stock < newItem.Quantity)
                    return new ServiceResult(false, $"Estoque insuficiente para o produto {product.Name}.");

                newItem.UnitPrice = product.Price;
                product.Stock -= newItem.Quantity;
            }

            // 3️⃣ Recalcula total
            sale.Total = sale.Items.Sum(i => i.Quantity * i.UnitPrice);

            // 4️⃣ Atualiza venda
            await _saleRepository.UpdateAsync(sale);
            await transaction.CommitAsync();

            return new ServiceResult(true, null, sale);
        }

        public async Task<ServiceResult> DeleteAsync(int id) {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var sale = await _saleRepository.GetByIdAsync(id);
            if (sale == null)
                return new ServiceResult(false, "Venda não encontrada.");

            // Reverte o estoque
            foreach (var item in sale.Items) {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                    product.Stock += item.Quantity;
            }

            var result = await _saleRepository.DeleteAsync(id);
            if (!result)
                return new ServiceResult(false, "Erro ao deletar a venda.");

            await transaction.CommitAsync();
            return new ServiceResult(true, null, sale);
        }
    }
}
