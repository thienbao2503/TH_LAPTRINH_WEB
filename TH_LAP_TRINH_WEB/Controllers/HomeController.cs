using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;
using TH_LAP_TRINH_WEB.Services;

namespace TH_LAP_TRINH_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly CartService _cartService;

        public HomeController(ILogger<HomeController> logger,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            CartService cartService)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest request)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid request" });

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            _cartService.AddItem(product, request.Quantity);
            return Json(new { success = true, cartTotal = _cartService.GetTotal() });
        }

        [HttpGet]
        public IActionResult GetCartSummary()
        {
            var cart = _cartService.GetCart();
            return Json(new
            {
                items = cart,
                total = _cartService.GetTotal()
            });
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveItem(productId);
            return Json(new { success = true });
        }

        public IActionResult Cart()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
