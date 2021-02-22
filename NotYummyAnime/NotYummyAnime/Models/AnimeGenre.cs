using System;
using System.Collections.Generic;

#nullable disable

namespace NotYummyAnime
{
    public partial class AnimeGenre
    {
        public int Id { get; set; }
        public int AnimeInfoId { get; set; }
        public int GenreId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
