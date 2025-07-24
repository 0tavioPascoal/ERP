using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers {
    public class ClientController : Controller {
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
    }
}
