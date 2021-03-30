using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotYummyAnime.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Range(18, 100 , ErrorMessage ="Wrong age")]
        [Display(Name = "Вік")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password" , ErrorMessage ="Паролі не співпадають")]
        [Display(Name = "Підтвердження паролю")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
