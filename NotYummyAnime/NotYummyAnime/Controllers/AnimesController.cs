using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotYummyAnime;

namespace NotYummyAnime.Controllers
{
    public class AnimesController : Controller
    {
        private readonly DBLibraryContext _context;

        public AnimesController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Animes
        public async Task<IActionResult> Index(int? ID , string name)
        {
            if (ID == null)
                return RedirectToAction("Index", "Genres");

            ViewBag.Genre_ID = ID;
            ViewBag.GenreName = name;

            var animeInfoByGenre = _context.AnimeGenres.Where(ag => ag.GenreId == ID).Select(ag => ag.AnimeInfoId);
            var animeByGenre = _context.Animes.Where(an => animeInfoByGenre.Contains(an.AnimeInfoId)).Include(an => an.AnimeInfo);
            
            return View(await animeByGenre.ToListAsync());
        }

        // GET: Animes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .Include(a => a.AnimeInfo)
                .FirstOrDefaultAsync(m => m.AnimeId == id);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }

        // GET: Animes/Create
        public IActionResult Create()
        {
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description");
            return View();
        }

        // POST: Animes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimeId,Poster,AnimeName,Rating,AnimeInfoId")] Anime anime)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(anime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description", anime.AnimeInfoId);
            return View(anime);
        }

        // GET: Animes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes.FindAsync(id);
            if (anime == null)
            {
                return NotFound();
            }
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description", anime.AnimeInfoId);
            return View(anime);
        }

        // POST: Animes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimeId,Poster,AnimeName,Rating,AnimeInfoId")] Anime anime)
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
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description", anime.AnimeInfoId);
            return View(anime);
        }

        // GET: Animes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anime = await _context.Animes
                .Include(a => a.AnimeInfo)
                .FirstOrDefaultAsync(m => m.AnimeId == id);
            if (anime == null)
            {
                return NotFound();
            }

            return View(anime);
        }

        // POST: Animes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            _context.Animes.Remove(anime);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeExists(int id)
        {
            return _context.Animes.Any(e => e.AnimeId == id);
        }
    }
}
