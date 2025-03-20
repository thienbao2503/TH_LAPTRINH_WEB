using System.ComponentModel.DataAnnotations;

namespace TH_LAP_TRINH_WEB.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Range(10000, 10000000.00)]
        [Display(Name = "Giá sản phẩm")]
        public decimal Price { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Hình ảnh chính")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Danh sách hình ảnh")]
        public List<ProductImage>? Images { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
