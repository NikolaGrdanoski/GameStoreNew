using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public ICollection<GameCategory>? GameCategories { get; set; }
    }
}
