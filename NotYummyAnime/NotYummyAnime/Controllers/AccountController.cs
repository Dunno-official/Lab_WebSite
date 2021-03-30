using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using NotYummyAnime.ViewModels;
using NotYummyAnime.Models;


namespace NotYummyAnime.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Year = model.Year,
                    Hash = Tools.Verification.GenerateHash()
                };
                // додаємо користувача
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    Tools.Verification.SendVerificationMessage(user.Id , user.Hash , model.Email);

                    return RedirectToAction("Profile");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // перевіряємо, чи належить URL додатку
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Profile");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін чи (та) пароль");
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // видаляємо аутентифікаційні куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Profile");
        }


        [HttpGet]
        public async Task<IActionResult> Verification(string id , string hash)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            bool verificationSucceeded = (user.Hash == hash);

            ViewBag.PseudoHash = hash;
            ViewBag.ID = id;
            ViewBag.VerificationSucceeded = verificationSucceeded;

            if (verificationSucceeded == true)
            {
                user.IsVerified = true;
                await _userManager.UpdateAsync(user);
            }

            return View();
        }


        [HttpGet]
        public IActionResult Profile()
        {
            return View(_userManager);
        }
    }
}
