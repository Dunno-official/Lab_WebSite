using System;
using System.Collections.Generic;

#nullable disable

namespace NotYummyAnime
{
    public partial class Genre
    {
        public Genre()
        {
            AnimeGenres = new HashSet<AnimeGenre>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
    }
}
