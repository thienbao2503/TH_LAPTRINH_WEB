using Microsoft.EntityFrameworkCore;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Models;

namespace TH_LAP_TRINH_WEB.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task AddAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Lỗi khi lưu vào database: {ex.InnerException?.Message}");
                throw;
            }
        }
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

    }
}
