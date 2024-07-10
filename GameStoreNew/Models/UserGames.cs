using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class UserGames
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string StoreUser { get; set; }

        [Required]
        [Display(Name = "Game")]
        public int GameId { get; set; }

        public Game? Game { get; set; }
    }
}
