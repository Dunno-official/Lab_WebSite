using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotYummyAnime.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string Hash { get; set; }
        public bool IsVerified { get; set; }
    }
}
