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

        public async Task<IActionResult> EditSale(int id) {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null) {
                return NotFound();
            }

            // Carregar listas para dropdowns
            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Products = await _context.Products.Where(p => p.Stock > 0).ToListAsync();
            ViewBag.PaymentMethods = new List<string> { "Dinheiro", "Cartão", "Pix" };

            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSale(Sale sale) {
            if (ModelState.IsValid) {
                await _saleService.UpdateAsync(sale); 
                return RedirectToAction("Index");
            }

            ViewBag.Clients = await _context.Clients.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            ViewBag.PaymentMethods = new List<string> { "Dinheiro", "Cartão", "Pix" };

            return View(sale);
        }

        public async Task<IActionResult> DeleteSale(int id) {
            var delete = await _saleService.DeleteAsync(id);
            if (delete == null )
                return NotFound();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id) {

            var client = await _saleService.GetByIdAsync(id);


            return View(client);
        }

    }
}
