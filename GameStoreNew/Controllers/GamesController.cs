using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStoreNew.Models;
using System.Data;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using GameStoreNew.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GameStoreNew.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameStoreNewContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public GamesController(GameStoreNewContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            hostingEnvironment = hostingEnvironment;
        }

        // GET: Games
        public async Task<IActionResult> Index(string searchString, string searchDeveloper)
        {
            IQueryable<Game> games = _context.Game.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(g => g.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(searchDeveloper))
            {
                games = games.Where(g => g.Developer.DeveloperName.Contains(searchString));
            }

            games = games.Include(g => g.Developer).Include(g => g.GameCategories).Include("GameCategories.Category");

            //var gameStoreNewContext = _context.Game.Include(g => g.Developer);
            return View(await games.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "Id", "DeveloperName");
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,YearPublished,Description,GameArt,DownloadURL,DeveloperId")] Game game, List<int> CategoryId)
        {
            if (ModelState.IsValid)
            {
                game.GameCategories = new List<GameCategory>();
                CategoryId.ForEach(c =>
                {
                    /*if(game.GameArt != null)
                    {
                        string fileName = imageFile(gameArtViewModel);
                        game.GameArt = fileName;
                    }*/
                    var categoryId = c;
                    var category = _context.Category.FirstOrDefault(ca => ca.Id == categoryId);
                    var gameCategory = new GameCategory { GameId = game.Id, Game = game, CategoryId = categoryId, Category = category };
                    game.GameCategories.Add(gameCategory);
                    _context.Add(gameCategory);
                });
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "Id", "DeveloperName", game.DeveloperId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View(game);

            /*if(ModelState.IsValid)
            {
                string fileName = imageFile(gameArtViewModel);

                Game game = new Game
                {
                    Name = gameArtViewModel.Name,
                    YearPublished = gameArtViewModel.YearPublished,
                    Description = gameArtViewModel.Description,
                    GameArt = fileName,
                    DownloadURL = gameArtViewModel.DownloadURL,
                    GameCategories = gameArtViewModel.GameCategories,
                    Developer = gameArtViewModel.Developer,
                };

                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }*/

            //return View();
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Set<Developer>(), "Id", "DeveloperName", game.DeveloperId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,YearPublished,Description,GameArt,DownloadURL,DeveloperId")] Game game, List<int> CategoryId)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var gameCategories = new List<GameCategory>();

                    game.GameCategories = _context.GameCategory.Where(g => g.GameId == game.Id).ToList();

                    for (int i = 0; i < game.GameCategories.Count; i++)
                    {
                        var g = _context.GameCategory.Find(game.GameCategories.ElementAt(i).Id);
                        _context.GameCategory.Remove(g);
                    }

                    CategoryId.ForEach(g =>
                    {
                        var categoryId = g;
                        var category = _context.Category.FirstOrDefault(gc => gc.Id == categoryId);
                        var gCategory = new GameCategory();

                        gCategory.Category = category;
                        gCategory.CategoryId = categoryId;
                        gCategory.GameId = game.Id;
                        gCategory.Game = game;
                        gameCategories.Add(gCategory);
                        _context.Add(gCategory);
                    });

                    game.GameCategories.Clear();
                    game.GameCategories = gameCategories;

                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Set<Developer>(), "Id", "DeveloperName", game.DeveloperId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.Id == id);
        }

        /*private string imageFile(GameArtViewModel gameArtViewModel)
        {
            string fileName = null;

            if (gameArtViewModel.Image != null)
            {
                string fileDir = Path.Combine(hostingEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(gameArtViewModel.Image.FileName);
                string filePath = Path.Combine(fileDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    gameArtViewModel.Image.CopyTo(fileStream);
                }
            }

            return fileName;
        }*/

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Buy(int id)
        {
            var userId = User.Identity.Name;
            var userGame = new UserGames
            {
                StoreUser = userId,
                GameId = id
            };

            _context.UserGames.Add(userGame);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
