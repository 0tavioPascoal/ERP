using ERP.Models;
using ERP.Services.Graphic;
using ERP.Services.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Controllers
{
    public class HomeController : Controller
    {

        private readonly SalesGraphicService salesGraphicService;

        public HomeController(SalesGraphicService salesGraphicService)
        {
            this.salesGraphicService = salesGraphicService;
        }

        public IActionResult Index()
        {

            var salesGraphic = salesGraphicService.GetSalesGraphic(360);
            return View(salesGraphic);
        
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
