using Microsoft.AspNetCore.Mvc;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Controllers
{
    public class CategoryListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryListController(ApplicationDbContext context, IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewData["SelectedCategoryId"] = categoryId;
            var selectedCategory = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            ViewData["SelectedCategoryName"] = selectedCategory?.Name;
            var products = await _productRepository.GetAllAsync();
            products = categoryId == null ? products.ToList() : products.Where(p => p.CategoryId == categoryId).ToList();
            return View(products);
        }

    }
}
