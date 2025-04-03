using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
            return View(new Product()); // Pass empty product model
        }
        // Xử lý thêm sản phẩm mới
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Name,Price,Description,CategoryId")] Product product, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (images != null && images.Count > 0)
                    {
                        product.Images = new List<ProductImage>();
                        foreach (var image in images)
                        {
                            string imageUrl = await SaveImage(image);
                            product.Images.Add(new ProductImage { Url = imageUrl });
                        }
                    }

                    await _productRepository.AddAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving product: " + ex.Message);
                }
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        //private async Task<string> SaveImage(IFormFile image)
        //{
        //    var imagesFolder = Path.Combine("wwwroot/images");
        //    if (!Directory.Exists(imagesFolder))
        //    {
        //        Directory.CreateDirectory(imagesFolder);
        //    }

        //    var existingFiles = Directory.GetFiles(imagesFolder, "IMG*.jpg");
        //    int nextId = existingFiles.Select(f => int.TryParse(Path.GetFileNameWithoutExtension(f).Substring(3), out int num) ? num : 0).DefaultIfEmpty(0).Max() + 1;
        //    string newFileName = $"IMG{nextId:D5}.jpg";
        //    var savePath = Path.Combine(imagesFolder, newFileName);

        //    using (var fileStream = new FileStream(savePath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(fileStream);
        //    }

        //    return "/images/" + newFileName;
        //}

        private async Task<string> SaveImage(IFormFile image)
        {
            var imagesFolder = Path.Combine("wwwroot/images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Get all existing images to generate a new unique name
            var existingFiles = Directory.GetFiles(imagesFolder, "IMG*.jpg");
            int nextId = existingFiles.Select(f => int.TryParse(Path.GetFileNameWithoutExtension(f).Substring(3), out int num) ? num : 0)
                                      .DefaultIfEmpty(0)
                                      .Max() + 1;

            // Create a new filename with a padded number (IMG00001.jpg)
            string newFileName = $"IMG{nextId:D5}.jpg";
            var savePath = Path.Combine(imagesFolder, newFileName);

            // Save the image to disk
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

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
        //[HttpPost]
        //public async Task<IActionResult> Update(int id, Product product, IFormFile imageUrl)
        //{
        //    ModelState.Remove("ImageUrl"); // Bỏ qua xác thực ModelState cho ImageUrl

        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var existingProduct = await _productRepository.GetByIdAsync(id);
        //        if (existingProduct == null)
        //        {
        //            return NotFound();
        //        }

        //        if (imageUrl != null)
        //        {
        //            // Xóa ảnh cũ nếu có
        //            if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
        //            {
        //                var oldImagePath = Path.Combine("wwwroot", existingProduct.ImageUrl.TrimStart('/'));
        //                if (System.IO.File.Exists(oldImagePath))
        //                {
        //                    System.IO.File.Delete(oldImagePath);
        //                }
        //            }

        //            // Lưu ảnh mới
        //            existingProduct.ImageUrl = await SaveImage(imageUrl);
        //        }

        //        // Cập nhật thông tin sản phẩm
        //        existingProduct.Name = product.Name;
        //        existingProduct.Price = product.Price;
        //        existingProduct.Description = product.Description;
        //        existingProduct.CategoryId = product.CategoryId;

        //        await _productRepository.UpdateAsync(existingProduct);

        //        return RedirectToAction(nameof(Index));
        //    }

        //    var categories = await _categoryRepository.GetAllAsync();
        //    ViewBag.Categories = new SelectList(categories, "Id", "Name");
        //    return View(product);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product, List<IFormFile> images)
        {
            ModelState.Remove("ImageUrl"); // Ignore validation for ImageUrl

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

                // Handle new images
                if (images != null && images.Count > 0)
                {
                    // Check image count limit
                    if (images.Count > 5)
                    {
                        ModelState.AddModelError("", "Bạn không thể tải lên quá 5 hình ảnh.");
                        return View(product);
                    }

                    // Delete old images from the file system
                    foreach (var image in existingProduct.Images)
                    {
                        var oldImagePath = Path.Combine("wwwroot", image.Url.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Clear existing images in database
                    existingProduct.Images.Clear();

                    // Save new images and update the database
                    foreach (var image in images)
                    {
                        string imageUrl = await SaveImage(image);
                        existingProduct.Images.Add(new ProductImage { Url = imageUrl });
                    }
                }

                // Update product information (non-image fields)
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;

                await _productRepository.UpdateAsync(existingProduct);

                return RedirectToAction(nameof(Index));
            }

            // Load categories for the dropdown
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

            var productImages = await _productRepository.GetProductImagesByProductIdAsync(id);
            if (productImages.Any())
            {
                foreach (var image in productImages)
                {
                    var imagePath = Path.Combine("wwwroot", image.Url.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                await _productRepository.DeleteProductImagesAsync(productImages);
            }

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var mainImagePath = Path.Combine("wwwroot", product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(mainImagePath))
                {
                    System.IO.File.Delete(mainImagePath);
                }
            }

            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
