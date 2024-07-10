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
    public class UserFavoritesController : Controller
    {
        private readonly GameStoreNewContext _context;

        public UserFavoritesController(GameStoreNewContext context)
        {
            _context = context;
        }

        // GET: UserFavorites
        public async Task<IActionResult> Index()
        {
            var gameStoreNewContext = _context.UserFavorites.Include(u => u.Game);
            return View(await gameStoreNewContext.ToListAsync());
        }

        // GET: UserFavorites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFavorites = await _context.UserFavorites
                .Include(u => u.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFavorites == null)
            {
                return NotFound();
            }

            return View(userFavorites);
        }

        // GET: UserFavorites/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name");
            return View();
        }

        // POST: UserFavorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreUser,GameId")] UserFavorites userFavorites)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userFavorites);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", userFavorites.GameId);
            return View(userFavorites);
        }

        // GET: UserFavorites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFavorites = await _context.UserFavorites.FindAsync(id);
            if (userFavorites == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", userFavorites.GameId);
            return View(userFavorites);
        }

        // POST: UserFavorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreUser,GameId")] UserFavorites userFavorites)
        {
            if (id != userFavorites.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userFavorites);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserFavoritesExists(userFavorites.Id))
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
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", userFavorites.GameId);
            return View(userFavorites);
        }

        // GET: UserFavorites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFavorites = await _context.UserFavorites
                .Include(u => u.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFavorites == null)
            {
                return NotFound();
            }

            return View(userFavorites);
        }

        // POST: UserFavorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFavorites = await _context.UserFavorites.FindAsync(id);
            if (userFavorites != null)
            {
                _context.UserFavorites.Remove(userFavorites);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserFavoritesExists(int id)
        {
            return _context.UserFavorites.Any(e => e.Id == id);
        }
    }
}
