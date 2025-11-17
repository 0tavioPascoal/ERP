using ERP.Context;
using ERP.Models;
using ERP.Repositories.Interfaces.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Repositories.Clients {
    public class ClientRepository : IClientRepository {

        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext appDbContext) {
          _context = appDbContext;
        }

        public async Task<Client> CreateClientAsync(Client client) {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> DeleteClientAsync(int id) {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
           return true;
        }

        public async Task<Client> EditClientAsync(Client client) {
            var existingClient = await _context.Clients.FindAsync(client.Id);
            if (existingClient == null) {
                return null;
            }

            existingClient.Name = client.Name;
            existingClient.Email = client.Email;
            existingClient.Phone = client.Phone;

            await _context.SaveChangesAsync();
            return existingClient;
        }

        public Task<Client> GetClientByIdAsync(int id) {
            return _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Client>> GetCLients() {
           return await _context.Clients.ToListAsync();
        }

    }
}
