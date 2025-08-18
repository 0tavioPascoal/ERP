using ERP.Context;
using ERP.Models;
using ERP.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Controllers {
    public class SaleController : Controller {

        private readonly SaleService _saleService;
        private readonly AppDbContext _context;

        public SaleController(SaleService saleService, AppDbContext context) {
            _saleService = saleService;
            _context = context;
        }

        public async Task<IActionResult> Index() {

            var sales = await _saleService.GetAllAsync();
            return View(sales);
        }

        public  async Task<IActionResult> CreateSale() {

            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Products = await _context.Products.Where(p => p.Stock > 0).ToListAsync();
            ViewBag.PaymentMethods = new List<string> { "Dinheiro", "Cartão", "Pix" };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSale(Sale sale) {
            if (!ModelState.IsValid) {
                // Recarrega dropdowns se houver erro
                ViewBag.Clients = await _context.Clients.ToListAsync();
                ViewBag.Products = await _context.Products.Where(p => p.Stock > 0).ToListAsync();
                ViewBag.PaymentMethods = new List<string> { "Dinheiro", "Cartão", "Pix" };
                return View(sale);
            }

            var result = await _saleService.AddAsync(sale);
            if (!result.Success) {
                ModelState.AddModelError("", result.ErrorMessage);
                ViewBag.Clients = await _context.Clients.ToListAsync();
                ViewBag.Products = await _context.Products.Where(p => p.Stock > 0).ToListAsync();
                ViewBag.PaymentMethods = new List<string> { "Dinheiro", "Cartão", "Pix" };
                return View(sale);
            }

            return RedirectToAction("Index");
        }
    }
}
