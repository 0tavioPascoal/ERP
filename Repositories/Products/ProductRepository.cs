using ERP.Context;
using ERP.Models;
using ERP.Repositories.Interfaces.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Repositories.Products {
    public class ProductRepository : IProductRepository {

        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync() {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id) {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product) {
            product.Created = DateTime.Now;
            product.Modified = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> UpdateAsync(Product product) {
            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null) {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.Modified = DateTime.Now;


            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteAsync(int id) {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
    }
}
