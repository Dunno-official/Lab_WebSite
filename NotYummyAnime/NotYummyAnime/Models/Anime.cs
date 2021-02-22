using System;
using System.Collections.Generic;

#nullable disable

namespace NotYummyAnime
{
    public partial class Anime
    {
        public Anime()
        {
            Comments = new HashSet<Comment>();
        }

        public int AnimeId { get; set; }
        public string Poster { get; set; }
        public string AnimeName { get; set; }
        public int Rating { get; set; }
        public int AnimeInfoId { get; set; }

        public virtual AnimeInfo AnimeInfo { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
