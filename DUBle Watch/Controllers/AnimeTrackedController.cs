using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DUBle_Watch.Data;
using DUBle_Watch.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DUBle_Watch.Controllers
{
    public class AnimeTrackedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnimeTrackedController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.AnimeTracked.Include(a => a.Anime).Where(x => x.UserId  == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> SortByEpisodesLeft()
        {
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.AnimeTracked.Include(a => a.Anime).OrderBy(a=>a.Anime.CurrentLastEpisode-a.CurrentEpisode);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> SortByFinished()
        {
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.AnimeTracked.Include(a => a.Anime).OrderBy(a => a.Anime.hasAnimeEnded);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> SortByReleaseDate()
        {
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.AnimeTracked.Include(a => a.Anime).OrderBy(a => a.Anime.AnimeReleaseDate);
            return View(await applicationDbContext.ToListAsync());
        }
            



        public async Task<IActionResult> FinishedAnimeIndex()
        {
            var applicationDbContext = _context.AnimeTracked.Include(a => a.Anime).ThenInclude(a => a.Genre).Where(a => a.IsInCurrentlyCompletedSection == true);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AnimeTrackeds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeTracked = await _context.AnimeTracked
                .Include(a => a.Anime)
                .FirstOrDefaultAsync(m => m.AnimeTrackedId == id);
            if (animeTracked == null)
            {
                return NotFound();
            }

            return View(animeTracked);
        }

        // GET: AnimeTrackeds/Create
        public IActionResult Create()
        {
            ViewData["AnimeId"] = new SelectList(_context.Anime, "AnimeId", "Name");
            return View();
        }

        // POST: AnimeTrackeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimeTrackedId,AnimeId,UserId,CompletedCount,CurrentlyCompleted,CurrentEpisode")] AnimeTracked animeTracked)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animeTracked);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimeId"] = new SelectList(_context.Anime, "AnimeId", "Name", animeTracked.AnimeId);
            return View(animeTracked);
        }

        // GET: AnimeTrackeds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeTracked = await _context.AnimeTracked.FindAsync(id);
            if (animeTracked == null)
            {
                return NotFound();
            }
            ViewData["AnimeId"] = new SelectList(_context.Anime, "AnimeId", "Name", animeTracked.AnimeId);
            return View(animeTracked);
        }

        // POST: AnimeTrackeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimeTrackedId,AnimeId,UserId,CompletedCount,IsInCurrentlyCompletedSection,CurrentEpisode")] AnimeTracked animeTracked)
        {
            if (id != animeTracked.AnimeTrackedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animeTracked);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeTrackedExists(animeTracked.AnimeTrackedId))
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
            ViewData["AnimeId"] = new SelectList(_context.Anime, "AnimeId", "Name", animeTracked.AnimeId);
            return View(animeTracked);
        }

        // GET: AnimeTrackeds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeTracked = await _context.AnimeTracked
                .Include(a => a.Anime)
                .FirstOrDefaultAsync(m => m.AnimeTrackedId == id);
            if (animeTracked == null)
            {
                return NotFound();
            }

            return View(animeTracked);
        }

        // POST: AnimeTrackeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animeTracked = await _context.AnimeTracked.FindAsync(id);
            _context.AnimeTracked.Remove(animeTracked);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeTrackedExists(int id)
        {
            return _context.AnimeTracked.Any(e => e.AnimeTrackedId == id);
        }
    }
}
