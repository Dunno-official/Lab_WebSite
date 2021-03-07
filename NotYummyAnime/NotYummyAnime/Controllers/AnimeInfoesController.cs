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
    public class AnimeInfoesController : Controller
    {
        private readonly DBLibraryContext _context;

        public AnimeInfoesController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: AnimeInfoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnimeInfos.ToListAsync());
        }

        // GET: AnimeInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeInfo = await _context.AnimeInfos
                .FirstOrDefaultAsync(m => m.AnimeInfoId == id);
            if (animeInfo == null)
            {
                return NotFound();
            }

            return View(animeInfo);
        }

        // GET: AnimeInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimeInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AnimeInfoId,StudioName,Status,AgeRating,Type,Description,Source,Season")] AnimeInfo animeInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animeInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animeInfo);
        }

        // GET: AnimeInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeInfo = await _context.AnimeInfos.FindAsync(id);
            if (animeInfo == null)
            {
                return NotFound();
            }
            return View(animeInfo);
        }

        // POST: AnimeInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AnimeInfoId,StudioName,Status,AgeRating,Type,Description,Source,Season")] AnimeInfo animeInfo)
        {
            if (id != animeInfo.AnimeInfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animeInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimeInfoExists(animeInfo.AnimeInfoId))
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
            return View(animeInfo);
        }

        // GET: AnimeInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeInfo = await _context.AnimeInfos
                .FirstOrDefaultAsync(m => m.AnimeInfoId == id);
            if (animeInfo == null)
            {
                return NotFound();
            }

            return View(animeInfo);
        }

        // POST: AnimeInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animeInfo = await _context.AnimeInfos.FindAsync(id);

            var animes = _context.Animes.Where(an => an.AnimeInfoId == id);
            _context.Animes.RemoveRange(animes);

            foreach(Genre genre in _context.Genres)
            {
                var anGenToBeRemoved = genre.AnimeGenres.Where(anGen => anGen.AnimeInfoId == id);
                
                foreach (AnimeGenre anGenre in anGenToBeRemoved)
                {
                    genre.AnimeGenres.Remove(anGenre);
                }
            }

            var anGen = _context.AnimeGenres.Where(ag => ag.AnimeInfoId == id);
            _context.AnimeGenres.RemoveRange(anGen);
            _context.AnimeInfos.Remove(animeInfo);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeInfoExists(int id)
        {
            return _context.AnimeInfos.Any(e => e.AnimeInfoId == id);
        }
    }
}
