using ERP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Repositories.Interfaces.Products {
    public interface IProductRepository {


        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);


    }
}
