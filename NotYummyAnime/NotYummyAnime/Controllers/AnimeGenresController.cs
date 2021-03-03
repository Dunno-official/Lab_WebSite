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
    public class AnimeGenresController : Controller
    {
        private readonly DBLibraryContext _context;

        public AnimeGenresController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: AnimeGenres
        public async Task<IActionResult> Index()
        {
            var dBLibraryContext = _context.AnimeGenres.Include(a => a.AnimeInfo).Include(a => a.Genre);
            return View(await dBLibraryContext.ToListAsync());
        }

        // GET: AnimeGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeGenre = await _context.AnimeGenres
                .Include(a => a.AnimeInfo)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animeGenre == null)
            {
                return NotFound();
            }

            return View(animeGenre);
        }

        // GET: AnimeGenres/Create
        public IActionResult Create(int? genreID, string genreName)
        {
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewBag.Genre_ID = genreID;
            ViewBag.GenreName = genreName;
            return View();
        }

        // POST: AnimeGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnimeInfoId,GenreId")] AnimeGenre animeGenre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animeGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description", animeGenre.AnimeInfoId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", animeGenre.GenreId);
            return View(animeGenre);
        }

        // GET: AnimeGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeGenre = await _context.AnimeGenres.FindAsync(id);
            if (animeGenre == null)
            {
                return NotFound();
            }
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description", animeGenre.AnimeInfoId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", animeGenre.GenreId);
            return View(animeGenre);
        }

        // POST: AnimeGenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AnimeInfoId,GenreId")] AnimeGenre animeGenre)
        {
            if (id != animeGenre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animeGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeGenreExists(animeGenre.Id))
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
            ViewData["AnimeInfoId"] = new SelectList(_context.AnimeInfos, "AnimeInfoId", "Description", animeGenre.AnimeInfoId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", animeGenre.GenreId);
            return View(animeGenre);
        }

        // GET: AnimeGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeGenre = await _context.AnimeGenres
                .Include(a => a.AnimeInfo)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animeGenre == null)
            {
                return NotFound();
            }

            return View(animeGenre);
        }

        // POST: AnimeGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animeGenre = await _context.AnimeGenres.FindAsync(id);
            _context.AnimeGenres.Remove(animeGenre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeGenreExists(int id)
        {
            return _context.AnimeGenres.Any(e => e.Id == id);
        }
    }
}
