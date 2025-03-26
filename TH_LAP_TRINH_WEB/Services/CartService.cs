using System.Text.Json;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = session.GetString(CartKey);
            return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        public void AddItem(Product product, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
        }

        public void RemoveItem(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }
        }

        public decimal GetTotal()
        {
            return GetCart().Sum(item => item.Total);
        }
    }
}
