using ERP.Models;
using System.Threading.Tasks;

namespace ERP.Repositories.Interfaces.Clients {
    public interface IClientInterface {

        Task<Client> CreateClientAsync(Client client);

    }
}
