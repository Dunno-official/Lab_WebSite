using System;
using System.Collections.Generic;

#nullable disable

namespace NotYummyAnime
{
    public partial class AnimeUser
    {
        public AnimeUser()
        {
            Comments = new HashSet<Comment>();
        }

        public int AnimeUserId { get; set; }
        public string NickName { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public int NumberOfRatedAnime { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
