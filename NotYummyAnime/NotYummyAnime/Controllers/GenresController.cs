using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotYummyAnime;

namespace NotYummyAnime.Controllers
{
    public class GenresController : Controller
    {
        private readonly DBLibraryContext _context;

        public GenresController(DBLibraryContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Genres.ToListAsync());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }


            //return View(genre);
            return RedirectToAction("Index", "Animes", new { ID = id, name = genre.GenreName });
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenreId,GenreName,Description")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreId,GenreName,Description")] Genre genre)
        {
            if (id != genre.GenreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.GenreId))
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
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid == false || fileExcel == null)
                return RedirectToAction(nameof(Index));

            using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
            {
                await fileExcel.CopyToAsync(stream);

                using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                {
                    var worksheet = workBook.Worksheet(1);
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
                    ReadAnime(rows);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private void ReadAnime(IEnumerable<IXLRangeRow> rows)
        {
            Anime anime = new Anime(); 
            AnimeInfo animeInfo = new AnimeInfo();
            string[] genres;

            foreach (var row in rows)
            {
                anime.Poster = row.Cell(1).Value.ToString();
                anime.AnimeName = row.Cell(2).Value.ToString();
                anime.Rating = Convert.ToInt32(row.Cell(3).Value);

                animeInfo.StudioName = row.Cell(4).Value.ToString();
                animeInfo.Status = row.Cell(5).Value.ToString();
                animeInfo.AgeRating = row.Cell(6).Value.ToString();
                animeInfo.Type = row.Cell(7).Value.ToString();
                animeInfo.Description = row.Cell(8).Value.ToString();
                animeInfo.Source = row.Cell(9).Value.ToString();
                animeInfo.Season = row.Cell(10).Value.ToString();

                genres = row.Cell(11).Value.ToString().Split(',');

                if (_context.Animes.Any(an => an.AnimeName == anime.AnimeName) == false) // if anime is already exists
                {
                    int animeInfoID = AddAnime(anime, animeInfo);
                }
            }
        }


        private int AddAnime(Anime anime , AnimeInfo animeInfo)
        {
            int newAnimeInfoID;

            _context.AnimeInfos.Add(animeInfo);
            newAnimeInfoID = _context.AnimeInfos.Last().AnimeInfoId;

            anime.AnimeInfoId = newAnimeInfoID;
            anime.AnimeInfo = animeInfo;

            return newAnimeInfoID;
        }


        private void AddNewGenres(string[] genres)
        {
            var currentGenresName = _context.Genres.Select(genre => genre.GenreName);
            var newGenres = genres.Except(currentGenresName);

            foreach (string newGenreName in newGenres)
            {
                _context.Genres.Add(new Genre { GenreName = newGenreName });
                int newGenreID = _context.Genres.Last().GenreId;
            }
        }


        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.GenreId == id);
        }
    }
}
