using BlogApplication.Areas.AdminArea.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApplication.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class AdminController : Controller
    {

        private readonly IAdminServices _adminservices;

        public AdminController(IAdminServices adminService)
        {
            _adminservices = adminService;
        }
        [Authorize(AuthenticationSchemes = "AdminAuth")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            
            if (_adminservices.AdminAuth(username,password))
            {
                Console.WriteLine(_adminservices.GetRole(username));
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, _adminservices.GetRole(username))
                };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminAuth");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("AdminAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Kullanıcı adı veya şifre yanlış!" });
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            // Cookie'yi silmek için SignOutAsync kullanılır
            await HttpContext.SignOutAsync("AdminAuth");

            // Çıkış yaptıktan sonra Admin giriş sayfasına yönlendir
            return RedirectToAction("Login", "Admin");
        }

    }
}
