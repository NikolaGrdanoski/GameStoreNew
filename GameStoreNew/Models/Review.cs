using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Game")]
        public int GameId { get; set; }

        public Game? Game { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "User")]
        public string StoreUser { get; set; }

        public int? Rating { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; }
    }
}
