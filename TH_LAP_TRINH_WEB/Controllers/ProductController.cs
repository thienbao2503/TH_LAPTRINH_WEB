using Microsoft.AspNetCore.Mvc;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Lấy thông tin sản phẩm theo ID và truyền vào view
        public async Task<IActionResult> Index(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy sản phẩm tương tự dựa trên category hoặc các thông tin khác và loại trừ sản phẩm hiện tại
            var relatedProducts = await _productRepository.GetRelatedProductsAsync(product.CategoryId);
            relatedProducts = relatedProducts.Where(p => p.Id != product.Id).ToList(); // Loại trừ sản phẩm hiện tại

            // Truyền sản phẩm hiện tại và sản phẩm liên quan vào view
            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }
    }
}
