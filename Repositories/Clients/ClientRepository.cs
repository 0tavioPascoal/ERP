using ERP.Context;
using ERP.Models;
using ERP.Repositories.Interfaces.Clients;
using System.Threading.Tasks;

namespace ERP.Repositories.Clients {
    public class ClientRepository : IClientInterface {

        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext appDbContext) {
          _context = appDbContext;
        }

        public async Task<Client> CreateClientAsync(Client client) {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
    }
}
