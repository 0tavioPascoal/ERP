using ERP.Models;
using ERP.Repositories.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.Controllers {
    public class ClientController : Controller {

        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientInterface) {
            _clientRepository = clientInterface;
        }

        public async Task<IActionResult> Index() {

            var clients = await _clientRepository.GetCLients();

            var viewModel = new ClientIndexViewModel {
                Clients = clients,
                NewClient = new Client()
            };

            return View(viewModel);
        }

      
        public IActionResult CreateClient() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(Client client) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _clientRepository.CreateClientAsync(client);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditClient(int id) {
            Client client = await _clientRepository.GetClientByIdAsync(id);
            return View(client);
        }


        [HttpPost]
        public async Task<IActionResult> EditClient(Client client) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedClient = await _clientRepository.EditClientAsync(client);

            if (updatedClient == null)
                return NotFound();

            return RedirectToAction("Index");
        }

    
        public async Task<IActionResult> DeleteClient(int id) {
            var deleted = await _clientRepository.DeleteClientAsync(id);
            if (!deleted)
            return NotFound();

            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> Delete(int id) {

            var client = await _clientRepository.GetClientByIdAsync(id);
      

            return View(client);
        }


    }
}
