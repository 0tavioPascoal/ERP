using ERP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Repositories.Interfaces.Clients {
    public interface IClientRepository {

        Task<Client> CreateClientAsync(Client client);
        List<Client> GetCLients();

    }
}
