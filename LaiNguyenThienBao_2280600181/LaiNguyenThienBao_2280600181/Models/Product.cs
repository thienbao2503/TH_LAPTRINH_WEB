using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LaiNguyenThienBao_2280600181.Models
{
    public class Product
    {
        [DisplayName("Mã")]
        public int Id { get; set; }
        [Required, StringLength(100), DisplayName("Tên")]
        public string Name { get; set; }
        [Range(1000, 10000000.00), DisplayName("Giá")]
        public decimal Price { get; set; }
        [DisplayName("Mô Tả")]
        public string Description { get; set; }
        [DisplayName("Danh Muc")]
        public int CategoryId { get; set; }
    }
}
