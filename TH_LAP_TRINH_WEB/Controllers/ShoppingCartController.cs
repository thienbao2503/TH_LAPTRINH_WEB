using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TH_LAP_TRINH_WEB.Extensions;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;
using TH_LAP_TRINH_WEB.Services;

namespace TH_LAP_TRINH_WEB.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CartService _cartService;

        public ShoppingCartController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IProductRepository productRepository, CartService cartService)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
        }

        public IActionResult Cart()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = _cartService.GetCart();
            if (cart == null || !cart.Any())
            {
                // Handle empty cart...
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.GetUserAsync(User);
            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _cartService.SaveCart(new List<CartItem>());

            return View("OrderCompleted", order.Id); // Order completion confirmation page
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            var product = _productRepository.GetByIdAsync(productId).Result;
            if (product != null)
            {
                _cartService.AddItem(product, quantity);
            }
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] CartUpdateModel model)
        {
            if (model == null || model.ProductId <= 0 || model.Quantity <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            _cartService.UpdateQuantity(model.ProductId, model.Quantity);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] CartUpdateModel model)
        {
            if (model == null || model.ProductId <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
            }

            _cartService.RemoveItem(model.ProductId);
            return Json(new { success = true });
        }

        public class CartUpdateModel
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}