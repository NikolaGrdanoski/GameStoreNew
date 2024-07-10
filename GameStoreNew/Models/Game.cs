using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Year Published")]
        public DateOnly? YearPublished { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Game Art")]
        public string? GameArt { get; set; }

        [Display(Name = "Download")]
        public string? DownloadURL { get; set; }

        [Required]
        [Display(Name = "Developer")]
        public int DeveloperId { get; set; }

        public Developer? Developer { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<UserGames>? Users { get; set; }

        public ICollection<GameCategory>? GameCategories { get; set; }
    }
}
