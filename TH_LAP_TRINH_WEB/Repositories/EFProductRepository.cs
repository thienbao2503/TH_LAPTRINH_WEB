using Microsoft.EntityFrameworkCore;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _context.Products
            .Include(p => p.Category)  // Include thông tin về category
            .Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                Category = p.Category,
                ImageUrl = p.Images != null && p.Images.Any() ? p.Images.FirstOrDefault().Url : null // Lấy ảnh đầu tiên
            })
            .ToListAsync();

            return products;
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category) // Lấy thông tin Category
                .Include(p => p.Images)   // Lấy tất cả ảnh liên quan đến sản phẩm
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImage>> GetProductImagesByProductIdAsync(int productId)
        {
            return await _context.ProductImages.Where(pi => pi.ProductId == productId).ToListAsync();
        }

        public async Task DeleteProductImagesAsync(List<ProductImage> images)
        {
            if (images.Any())
            {
                _context.ProductImages.RemoveRange(images);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetRelatedProductsAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId) // Lọc theo category hoặc tiêu chí nào đó
                .Take(4) // Lấy một số lượng sản phẩm nhất định
                .ToListAsync();
        }
    }
}
