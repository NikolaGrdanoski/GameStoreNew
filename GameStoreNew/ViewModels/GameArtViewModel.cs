using GameStoreNew.Models;
using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.ViewModels
{
    public class GameArtViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Year Published")]
        public DateOnly? YearPublished { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Game Art")]
        public IFormFile? GameArt { get; set; }

        [Display(Name = "Download")]
        public string? DownloadURL { get; set; }

        public Developer? Developer { get; set; }

        public ICollection<GameCategory>? GameCategories { get; set; }
    }
}
