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
    public class UserFavoriteDevelopersController : Controller
    {
        private readonly GameStoreNewContext _context;

        public UserFavoriteDevelopersController(GameStoreNewContext context)
        {
            _context = context;
        }

        // GET: UserFavoriteDevelopers
        public async Task<IActionResult> Index()
        {
            var gameStoreNewContext = _context.UserFavoriteDevelopers.Include(u => u.Developer);
            return View(await gameStoreNewContext.ToListAsync());
        }

        // GET: UserFavoriteDevelopers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFavoriteDevelopers = await _context.UserFavoriteDevelopers
                .Include(u => u.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFavoriteDevelopers == null)
            {
                return NotFound();
            }

            return View(userFavoriteDevelopers);
        }

        // GET: UserFavoriteDevelopers/Create
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "Id", "DeveloperName");
            return View();
        }

        // POST: UserFavoriteDevelopers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreUser,DeveloperId")] UserFavoriteDevelopers userFavoriteDevelopers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userFavoriteDevelopers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "Id", "DeveloperName", userFavoriteDevelopers.DeveloperId);
            return View(userFavoriteDevelopers);
        }

        // GET: UserFavoriteDevelopers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFavoriteDevelopers = await _context.UserFavoriteDevelopers.FindAsync(id);
            if (userFavoriteDevelopers == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "Id", "DeveloperName", userFavoriteDevelopers.DeveloperId);
            return View(userFavoriteDevelopers);
        }

        // POST: UserFavoriteDevelopers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreUser,DeveloperId")] UserFavoriteDevelopers userFavoriteDevelopers)
        {
            if (id != userFavoriteDevelopers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userFavoriteDevelopers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserFavoriteDevelopersExists(userFavoriteDevelopers.Id))
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
            ViewData["DeveloperId"] = new SelectList(_context.Developer, "Id", "DeveloperName", userFavoriteDevelopers.DeveloperId);
            return View(userFavoriteDevelopers);
        }

        // GET: UserFavoriteDevelopers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFavoriteDevelopers = await _context.UserFavoriteDevelopers
                .Include(u => u.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFavoriteDevelopers == null)
            {
                return NotFound();
            }

            return View(userFavoriteDevelopers);
        }

        // POST: UserFavoriteDevelopers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFavoriteDevelopers = await _context.UserFavoriteDevelopers.FindAsync(id);
            if (userFavoriteDevelopers != null)
            {
                _context.UserFavoriteDevelopers.Remove(userFavoriteDevelopers);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserFavoriteDevelopersExists(int id)
        {
            return _context.UserFavoriteDevelopers.Any(e => e.Id == id);
        }
    }
}
