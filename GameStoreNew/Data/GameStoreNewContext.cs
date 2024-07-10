using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GameStoreNew.Areas.Identity.Data;
using GameStoreNew.Models;

namespace GameStoreNew.Models
{
    public class GameStoreNewContext : IdentityDbContext<GameStoreNewUser>
    {
        public GameStoreNewContext (DbContextOptions<GameStoreNewContext> options)
            : base(options)
        {
        }

        public DbSet<GameStoreNew.Models.Game> Game { get; set; } = default!;
        public DbSet<GameStoreNew.Models.Category> Category { get; set; } = default!;
        public DbSet<GameStoreNew.Models.Developer> Developer { get; set; } = default!;
        public DbSet<GameStoreNew.Models.Review> Review { get; set; } = default!;
        public DbSet<GameStoreNew.Models.GameCategory> GameCategory { get; set; } = default!;
        public DbSet<GameStoreNew.Models.UserFavoriteDevelopers> UserFavoriteDevelopers { get; set; } = default!;
        public DbSet<GameStoreNew.Models.UserFavorites> UserFavorites { get; set; } = default!;
        public DbSet<GameStoreNew.Models.UserGames> UserGames { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
