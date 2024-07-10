using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class Developer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Developer")]
        public string DeveloperName { get; set; }

        public string? Country { get; set; }

        public ICollection<Game>? Games { get; set; }
    }
}
