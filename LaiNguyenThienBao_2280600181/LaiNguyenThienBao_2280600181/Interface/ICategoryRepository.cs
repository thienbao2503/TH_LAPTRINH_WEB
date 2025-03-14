using LaiNguyenThienBao_2280600181.Models;

namespace LaiNguyenThienBao_2280600181.Interface
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}
