using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotYummyAnime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChartsController : ControllerBase
    {
        private readonly DBLibraryContext _context;

        public ChartsController (DBLibraryContext context)
        {
            _context = context;
        }


        [HttpGet("JsonData")]
        public JsonResult JsonDataGenres()
        {
            var genres = _context.Genres.Include(genre => genre.AnimeGenres).ToList();

            List<object> genAnime = new List<object>();

            genAnime.Add(new[] { "Жанр", "Кількість аніме" });

            foreach (var g in genres)
            {
                genAnime.Add(new object[] { g.GenreName, g.AnimeGenres.Count() });
            }

            return new JsonResult(genAnime);
        }


        [HttpGet("JsonDataAnimeAgeRating")]
        public JsonResult JsonDataAnimeAgeRating()
        {
            List<string> ageRatings = _context.AnimeInfos.Select(an => an.AgeRating).ToList();
            ageRatings = ageRatings.Distinct().ToList();
            
            List<AnimeInfo> animeInfoes = _context.AnimeInfos.ToList();
            List<object> agRat = new List<object>();

            agRat.Add(new[] { "Вікова категорія", "Кількість аніме" });

            foreach (var ag in ageRatings)
            {
                int numOfAnime = animeInfoes.Count(an => an.AgeRating == ag);
                agRat.Add(new object[] { ag, numOfAnime });
            }

            return new JsonResult(agRat);
        }

    }
}
