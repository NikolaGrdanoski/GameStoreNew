using System.ComponentModel.DataAnnotations;

namespace GameStoreNew.Models
{
    public class GameCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GameId { get; set; }

        public Game? Game { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
