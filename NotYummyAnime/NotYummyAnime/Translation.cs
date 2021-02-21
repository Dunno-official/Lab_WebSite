using System;
using System.Collections.Generic;

#nullable disable

namespace NotYummyAnime
{
    public partial class Translation
    {
        public Translation()
        {
            AnimeTranslations = new HashSet<AnimeTranslation>();
        }

        public int TranslationId { get; set; }
        public string OrganizationName { get; set; }
        public string TranslationType { get; set; }

        public virtual ICollection<AnimeTranslation> AnimeTranslations { get; set; }
    }
}
