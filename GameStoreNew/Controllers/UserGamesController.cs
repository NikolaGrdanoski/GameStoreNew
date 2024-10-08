﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStoreNew.Models;
using Microsoft.AspNetCore.Authorization;
using GameStoreNew.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace GameStoreNew.Controllers
{
    public class UserGamesController : Controller
    {
        private readonly GameStoreNewContext _context;
        private readonly UserManager<GameStoreNewUser> userManager;

        public UserGamesController(GameStoreNewContext context)
        {
            _context = context;
            userManager = userManager;
        }

        // GET: UserGames
        public async Task<IActionResult> Index(string searchString)
        {
            //var gameStoreNewContext = _context.UserGames.Include(u => u.Game);
            //return View(await gameStoreNewContext.ToListAsync());

            var user = User.Identity.Name;
            IQueryable<UserGames> games = _context.UserGames.AsQueryable().Where(g => g.StoreUser == user);
            IQueryable<string> categoriesQuery = _context.Category.Distinct().Select(g => g.CategoryName).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                games = games.Where(g => g.Game.Name.Contains(searchString));
            }

            games = games.Include(g => g.Game).ThenInclude(g => g.Developer);

            //var gameStoreNewContext = _context.Game.Include(g => g.Developer);
            return View(await games.ToListAsync());
        }

            // GET: UserGames/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userGames = await _context.UserGames
                .Include(u => u.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userGames == null)
            {
                return NotFound();
            }

            return View(userGames);
        }

        // GET: UserGames/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name");
            return View();
        }

        // POST: UserGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreUser,GameId")] UserGames userGames)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userGames);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", userGames.GameId);
            return View(userGames);
        }

        // GET: UserGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userGames = await _context.UserGames.FindAsync(id);
            if (userGames == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", userGames.GameId);
            return View(userGames);
        }

        // POST: UserGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreUser,GameId")] UserGames userGames)
        {
            if (id != userGames.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userGames);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserGamesExists(userGames.Id))
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
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", userGames.GameId);
            return View(userGames);
        }

        // GET: UserGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userGames = await _context.UserGames
                .Include(u => u.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userGames == null)
            {
                return NotFound();
            }

            return View(userGames);
        }

        // POST: UserGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userGames = await _context.UserGames.FindAsync(id);
            if (userGames != null)
            {
                _context.UserGames.Remove(userGames);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserGamesExists(int id)
        {
            return _context.UserGames.Any(e => e.Id == id);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Return(int id)
        {
            //var user = await userManager.GetUserAsync(HttpContext.User);
            var userId = User.Identity.Name;

            if (id == null)
            {
                return NotFound(ModelState);
            }

            var game = _context.UserGames.Where(g => g.GameId == id && g.StoreUser == userId).FirstOrDefault();

            if (game == null)
            {
                return RedirectToAction(nameof(Index));
            }

            _context.UserGames.Remove(game);
            _context.SaveChanges();
            return RedirectToAction(nameof(UserGames));
        }
    }
}
