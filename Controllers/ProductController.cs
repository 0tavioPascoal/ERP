using ERP.Models;
using ERP.Repositories.Interfaces.Products;
using ERP.Repositories.Products;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ERP.Controllers {
    public class ProductController : Controller {

        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepo) {
            _productRepository = productRepo;
        }
        public async Task<IActionResult> Index() {

            var products = await _productRepository.GetAllAsync();

            return View(products);
        }

        public IActionResult CreateProduct() {
             return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            await _productRepository.AddAsync(product);
            return RedirectToAction("Index");
        }
    }
}
