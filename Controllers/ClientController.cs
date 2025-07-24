using ERP.Models;
using ERP.Repositories.Clients;
using ERP.Repositories.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP.Controllers {
    public class ClientController : Controller {

        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientInterface) {
            _clientRepository = clientInterface;
        }

        public IActionResult Index() {

             var viewModel = new ClientIndexViewModel {
                Clients = _clientRepository.GetCLients(),
                NewClient = new Client()
            };

            return View(viewModel);
        }

      
        public IActionResult CreateClient() {
            return View();
        }

        public IActionResult EditClient() {
            return View();
        }

        public IActionResult DeleteClient() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] Client client) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);  
            }

            await _clientRepository.CreateClientAsync(client);

            return RedirectToAction("Index");
        }  
    }
}
