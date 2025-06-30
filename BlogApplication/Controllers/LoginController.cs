using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginViewAsync(RegisterLoginViewModel model)
        {
            if (TryValidateModel(model.login))
            {
                return View(model);
            }

            
            if (_userService.UserAuth(model.login.Email, model.login.PasswordHash))
            {
                var user=_userService.GetUserbyEmail(model.login.Email);
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name,user.Username), // Kullanıcı adı
                new Claim(ClaimTypes.Email, user.Email),   // E-posta
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),//User Id
                new Claim("AuthenticationType", "user")
                };
                var claimsIdentity = new ClaimsIdentity(claims, "user");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync("user", new ClaimsPrincipal(claimsIdentity), authProperties );
               
               
                
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Geçersiz giriş. Lütfen e-posta ve şifrenizi kontrol edin.";
            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterLoginViewModel model)
        {
            if (TryValidateModel(model.user))
            {
                return View("LoginView", model);
            }

            _userService.AddUser(model.user);
            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("LoginView");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("user");
            return RedirectToAction("Index", "Home");
        }
    }
}
