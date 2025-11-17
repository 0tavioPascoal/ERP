using ERP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ERP.Controllers {
    public class AccountController : Controller {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) {
            return View(new LoginViewModel {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginView) {
            if (!ModelState.IsValid)
                return View(loginView);

            var user = await _userManager.FindByNameAsync(loginView.Username);

            if (user != null) {
                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    loginView.Password,
                    false,
                    false
                );

                if (result.Succeeded) {
                    if (string.IsNullOrEmpty(loginView.ReturnUrl))
                        return RedirectToAction("Index", "Home");

                    return Redirect(loginView.ReturnUrl);
                }
            }

            ModelState.AddModelError("", "Usuário ou senha inválidos");
            return View(loginView);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(LoginViewModel loginView) {

            if (!ModelState.IsValid)
                return View(loginView);

            var user = new IdentityUser {
                UserName = loginView.Username,
   
            };

            var result = await _userManager.CreateAsync(user, loginView.Password);

            if (result.Succeeded) {
                return RedirectToAction("Login", "Account");
            }

            // mostrar erros do Identity
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }

            return View(loginView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }
}