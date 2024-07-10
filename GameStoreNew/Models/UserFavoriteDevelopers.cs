using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class UserFavoriteDevelopers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string StoreUser { get; set; }

        [Required]
        [Display(Name = "Developer")]
        public int DeveloperId { get; set; }

        public Developer? Developer { get; set; }
    }
}
