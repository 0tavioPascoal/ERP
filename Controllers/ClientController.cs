using ERP.Models;
using ERP.Repositories.Clients;
using ERP.Repositories.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.Controllers {
    public class ClientController : Controller {

        private readonly IClientInterface _clientInterface;

        public ClientController(IClientInterface clientInterface) {
            _clientInterface = clientInterface;
        }

        public IActionResult Index() {
            return View();
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

            await _clientInterface.CreateClientAsync(client);

            return RedirectToAction("Index");
        }  
    }
}
