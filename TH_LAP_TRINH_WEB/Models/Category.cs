using System.ComponentModel.DataAnnotations;

namespace TH_LAP_TRINH_WEB.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }
    }
}
