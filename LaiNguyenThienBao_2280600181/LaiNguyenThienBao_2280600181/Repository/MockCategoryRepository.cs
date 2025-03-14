using LaiNguyenThienBao_2280600181.Interface;
using LaiNguyenThienBao_2280600181.Models;

namespace LaiNguyenThienBao_2280600181.Repository
{
    public class MockCategoryRepository : ICategoryRepository
    {
        private List<Category> _categoryList;
        public MockCategoryRepository()
        {
            _categoryList = new List<Category>
                {
                    new Category { Id = 1, Name = "Laptop" },
                    new Category { Id = 2, Name = "Desktop" },
                };
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryList;
        }
    }
}
