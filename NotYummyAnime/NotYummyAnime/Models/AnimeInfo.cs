using System;
using System.Collections.Generic;

#nullable disable

namespace NotYummyAnime
{
    public partial class AnimeInfo
    {
        public AnimeInfo()
        {
            AnimeGenres = new HashSet<AnimeGenre>();
            AnimeTranslations = new HashSet<AnimeTranslation>();
            Animes = new HashSet<Anime>();
        }

        public int AnimeInfoId { get; set; }
        public string StudioName { get; set; }
        public string Status { get; set; }
        public int AgeRating { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Season { get; set; }

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
        public virtual ICollection<AnimeTranslation> AnimeTranslations { get; set; }
        public virtual ICollection<Anime> Animes { get; set; }
    }
}
