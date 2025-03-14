using LaiNguyenThienBao_2280600181.Models;

namespace LaiNguyenThienBao_2280600181.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
