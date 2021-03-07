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
            string[] genres;

            foreach (var row in rows)
            {
                Anime anime = new Anime();
                AnimeInfo animeInfo = new AnimeInfo();

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
                    DistributeGenres(genres, animeInfoID);
                    _context.SaveChanges();
                }
            }
        }


        private int AddAnime(Anime anime , AnimeInfo animeInfo)
        {
            int newAnimeInfoID;
            animeInfo.Animes.Add(anime);

            animeInfo.AnimeInfoId = 0;
            _context.AnimeInfos.Add(animeInfo);
            _context.SaveChanges();
            newAnimeInfoID = animeInfo.AnimeInfoId;

            anime.AnimeId = 0;
            anime.AnimeInfo = animeInfo;
            anime.AnimeInfoId = newAnimeInfoID;

            _context.Animes.Add(anime);
            _context.SaveChanges();

            return newAnimeInfoID;
        }


        private void DistributeGenres(string[] genres , int animeInfoID)
        {
            Genre genre;
            AnimeInfo animeInfo = _context.AnimeInfos.Find(animeInfoID);

            foreach (string genreName in genres)
            {
                AnimeGenre animeGenre = new AnimeGenre { AnimeInfo = animeInfo, AnimeInfoId = animeInfoID };

                if (GenreExists(genreName) == false)
                {
                    genre = new Genre { GenreName = genreName };
                    _context.Genres.Add(genre);
                    _context.SaveChanges();
                }
                
                else
                {
                    genre = GetGenre(genreName);
                }

                animeGenre.Genre = genre;
                animeGenre.GenreId = genre.GenreId;

                _context.AnimeGenres.Add(animeGenre);
                _context.SaveChanges();

                genre.AnimeGenres.Add(animeGenre);
                animeInfo.AnimeGenres.Add(animeGenre);
            }
        }


        private Genre GetGenre(string genreName)
        {
            return _context.Genres.Where(gen => gen.GenreName == genreName).FirstOrDefault();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.GenreId == id);
        }

        private bool GenreExists(string genreName)
        {
            return _context.Genres.Any(gen => gen.GenreName == genreName);
        }
    }
}
