using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string GenreName { get; set; }

        [Display(Name = "Інформація")]
        public string Description { get; set; }
        

        public virtual ICollection<AnimeGenre> AnimeGenres { get; set; }
    }
}
