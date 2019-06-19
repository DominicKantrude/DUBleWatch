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
using DUBle_Watch.Models.AnimeViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DUBle_Watch.Controllers
{
    public class AnimeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnimeController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, 
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Anime
        public async Task<IActionResult> Index()
        {
            
           

            var user = await GetCurrentUserAsync();

           
            if(user != null)
            {

            }
            var userTrackedAnimeIds = _context.AnimeTracked.Include(a => a.Anime).Where(x => x.UserId == user.Id).Select(a=>a.Anime.AnimeId);

            var allAnimes = _context.Anime.Include(a => a.Genre);

            var availableAnimes = allAnimes.Where(a => !userTrackedAnimeIds.Contains(a.AnimeId));

            return View(await availableAnimes.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> AddToTracked(int id)
        {
            var user = await GetCurrentUserAsync();

            AnimeTracked animeTracked = new AnimeTracked()
            {
                AnimeId = id,
                UserId = user.Id,
                TimesCompleted = 0,
                IsInCurrentlyCompletedSection = false,
                CurrentEpisode = 0
            };

            var applicationDbContext = _context.AnimeTracked.Add(animeTracked);
            _context.SaveChanges();
           return RedirectToAction("Index", "Anime");
        }

        // GET: Anime/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Anime
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AnimeId == id);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }

        // GET: Anime/Create
        public IActionResult Create()
        {
            UploadImageViewModel viewAnime = new UploadImageViewModel();
            viewAnime.Anime = new Anime();
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "Name");
            return View(viewAnime);

        }

        // POST: Anime/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UploadImageViewModel viewAnime)
        {
            if (ModelState.IsValid)
            {

                if (viewAnime.ImageFile != null)
                {
                    
                    var fileName = Path.GetFileName(viewAnime.ImageFile.FileName);
                    Path.GetTempFileName();
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewAnime.ImageFile.CopyToAsync(stream);
                        // validate file, then move to CDN or public folder
                    }

                    viewAnime.Anime.ImagePath = viewAnime.ImageFile.FileName;
                }
                _context.Add(viewAnime.Anime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreId", viewAnime.Anime.GenreId);
            return View(viewAnime.Anime);
        }

        // GET: Anime/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Anime.FindAsync(id);
            if (anime == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreId", anime.GenreId);
            return View(anime);
        }

        // POST: Anime/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimeId,Name,CurrentLastEpisode,GenreId,AnimeLink,Description,AnimeReleaseDate, hasAnimeEnded")] Anime anime)
        {
            if (id != anime.AnimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeExists(anime.AnimeId))
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
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreId", anime.GenreId);
            return View(anime);
        }

        // GET: Anime/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Anime
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AnimeId == id);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }

        // POST: Anime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anime = await _context.Anime.FindAsync(id);
            _context.Anime.Remove(anime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeExists(int id)
        {
            return _context.Anime.Any(e => e.AnimeId == id);
        }
    }
}
