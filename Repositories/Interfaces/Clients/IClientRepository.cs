using ERP.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Repositories.Interfaces.Clients {
    public interface IClientRepository {

        Task<Client> CreateClientAsync(Client client);
        Task<List<Client>> GetCLients();

        Task<Client> GetClientByIdAsync(int id);
        Task<Client> EditClientAsync(Client client);
        Task<bool> DeleteClientAsync(int id);

    }
}
