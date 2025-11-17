using ERP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Repositories.Interfaces.Sales {
    public interface ISaleRepository {

        Task<IEnumerable<Sale>> GetAllAsync();
        Task<Sale?> GetByIdAsync(int id);
        Task AddAsync(Sale sale);
        Task<Sale> UpdateAsync(Sale sale);
        Task<bool> DeleteAsync(int id);
    }
}
