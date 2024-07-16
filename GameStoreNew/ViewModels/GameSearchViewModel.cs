using GameStoreNew.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStoreNew.ViewModels
{
    public class GameSearchViewModel
    {
        public IList<Game>? CurrentGames { get; set; }
        
        public SelectList? SelectGames { get; set; }

        public string searchString { get; set; }
    }
}
