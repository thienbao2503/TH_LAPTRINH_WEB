using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository; 
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile
        imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        // Viết thêm hàm SaveImage (tham khảo bài 02)
        private async Task<string> SaveImage(IFormFile image)
        {
            // Đường dẫn thư mục lưu ảnh
            var imagesFolder = Path.Combine("wwwroot/images");

            // Nếu thư mục chưa tồn tại, tạo mới
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Lấy danh sách file trong thư mục theo định dạng IMGxxxx
            var existingFiles = Directory.GetFiles(imagesFolder, "IMG*.jpg");

            // Xác định ID tiếp theo
            int nextId = existingFiles.Length + 1;

            // Sinh tên file mới với độ dài 4 chữ số (IMG0001, IMG0002, ...)
            string newFileName = $"IMG{nextId:D4}.jpg";

            // Đường dẫn lưu file
            var savePath = Path.Combine(imagesFolder, newFileName);

            // Lưu ảnh
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            // Trả về đường dẫn ảnh để lưu vào database
            return "/images/" + newFileName;
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name",
            product.CategoryId);
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl"); // Bỏ qua xác thực ModelState cho ImageUrl

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (imageUrl != null)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                    {
                        var oldImagePath = Path.Combine("wwwroot", existingProduct.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Lưu ảnh mới
                    existingProduct.ImageUrl = await SaveImage(imageUrl);
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                await _productRepository.UpdateAsync(existingProduct);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Xóa ảnh nếu tồn tại
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var imagePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Xóa sản phẩm khỏi database
            await _productRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


    }
}
