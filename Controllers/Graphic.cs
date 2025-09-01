using ERP.Services.Graphic;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers {
    public class Graphic : Controller {

        private readonly SalesGraphicService _salesGraphicService;

        public Graphic(SalesGraphicService salesGraphicService) {
            _salesGraphicService = salesGraphicService;
        }

        public JsonResult Sell(int dias) {
            var salesGraphic = _salesGraphicService.GetSalesGraphic(dias);
            return Json(salesGraphic);
        }
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult SellMonth(int dias) {
            return View();
        }

        [HttpGet]
        public IActionResult SellWeekend(int dias) {
            return View();
        }

        [HttpGet]
        public IActionResult SellYears(int dias) {
            return View();
        }
    }
}
