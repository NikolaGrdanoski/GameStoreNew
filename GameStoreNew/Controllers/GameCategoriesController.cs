using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStoreNew.Models;

namespace GameStoreNew.Controllers
{
    public class GameCategoriesController : Controller
    {
        private readonly GameStoreNewContext _context;

        public GameCategoriesController(GameStoreNewContext context)
        {
            _context = context;
        }

        // GET: GameCategories
        public async Task<IActionResult> Index()
        {
            var gameStoreNewContext = _context.GameCategory.Include(g => g.Category).Include(g => g.Game);
            return View(await gameStoreNewContext.ToListAsync());
        }

        // GET: GameCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.GameCategory
                .Include(g => g.Category)
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameCategory == null)
            {
                return NotFound();
            }

            return View(gameCategory);
        }

        // GET: GameCategories/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name");
            return View();
        }

        // POST: GameCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameId,CategoryId")] GameCategory gameCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", gameCategory.CategoryId);
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", gameCategory.GameId);
            return View(gameCategory);
        }

        // GET: GameCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.GameCategory.FindAsync(id);
            if (gameCategory == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", gameCategory.CategoryId);
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", gameCategory.GameId);
            return View(gameCategory);
        }

        // POST: GameCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,CategoryId")] GameCategory gameCategory)
        {
            if (id != gameCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameCategoryExists(gameCategory.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", gameCategory.CategoryId);
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", gameCategory.GameId);
            return View(gameCategory);
        }

        // GET: GameCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.GameCategory
                .Include(g => g.Category)
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameCategory == null)
            {
                return NotFound();
            }

            return View(gameCategory);
        }

        // POST: GameCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameCategory = await _context.GameCategory.FindAsync(id);
            if (gameCategory != null)
            {
                _context.GameCategory.Remove(gameCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameCategoryExists(int id)
        {
            return _context.GameCategory.Any(e => e.Id == id);
        }
    }
}
