using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Security.Claims;

namespace BlogApplication.Controllers
{
    [Authorize(AuthenticationSchemes = "user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IWebHostEnvironment environment,ILogger<UserController>logger) {
            _userService = userService;
            _environment = environment;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult UserView()
        {
            var _currentGuid = (User.FindFirstValue(ClaimTypes.NameIdentifier));
            Guid.TryParse(_currentGuid, out Guid userId);

            var user = _userService.GetUser(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditPhoto(IFormFile photo)
        {
            try
            {
                var _currentGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(_currentGuid, out Guid userId))
                {
                    ModelState.AddModelError("", "Kullanıcı bilgileri alınamadı.");
                    return View("UserView");
                }

                var user = _userService.GetUser(userId);

                // Fotoğraf yoksa uyarı
                if (photo == null || photo.Length == 0)
                {
                    ModelState.AddModelError("", "Lütfen bir dosya seçin.");
                    return View("UserView");
                }

                // Dosya uzantısı kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(Path.GetExtension(photo.FileName).ToLower()))
                {
                    ModelState.AddModelError("", "Sadece JPG, PNG veya GIF formatındaki dosyaları yükleyebilirsiniz.");
                    return View("UserView");
                }

                // Fotoğraf yükleme klasörü
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Dosya ismi ve yolunu oluşturma
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Fotoğrafı kaydetme
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // Veritabanındaki resim yolunu güncelleme
                user.proImage = "../uploads/"+fileName;
                _userService.EditPhoto(user);

                TempData["SuccessMessage"] = "Fotoğraf başarıyla yüklendi!";
                return RedirectToAction("UserView");
            }
            catch (Exception ex)
            {
                // Hata loglama (örneğin bir loglama servisi ile)
                _logger.LogError(ex, "Fotoğraf yükleme sırasında bir hata oluştu.");
                ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyin.");
                return View("UserView");
            }
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if(!ModelState.IsValid)
            {                
                    var _currentGuid = (User.FindFirstValue(ClaimTypes.NameIdentifier));
                    Guid.TryParse(_currentGuid, out Guid userId);

                    user.Id = _userService.GetUser(userId).Id;

                    if (user.Username == null||user.Email==null)
                    {
                        ModelState.AddModelError("", "Kullanıcı bilgileri geçersiz.");
                        return View("UserView", user);
                    }
                    // Kullanıcıyı güncelle
                    _userService.UpdateUser(user);

                    TempData["SuccessMessage"] = "Profil başarıyla güncellendi!";
                    return RedirectToAction("UserView");    
            }
            return View("UserView", user);
        }
        [HttpPost]
        public IActionResult EditPass(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            
            var _currentGuid = (User.FindFirstValue(ClaimTypes.NameIdentifier));
            Guid.TryParse(_currentGuid, out Guid userId);
            var user = _userService.GetUser(userId);


            if (CurrentPassword == null || NewPassword == null || ConfirmPassword == null)
            {
                TempData["ErrorMessage"] = "Tüm Alanları Doldurunuz";
                return View("UserView", user);
            }
            else if (NewPassword != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Yeni Şifre ve  Tekrar Birbirine Uymuyor";
                return RedirectToAction("UserView", "User");
            }
            else if(_userService.UserAuth(user.Email,CurrentPassword))
            {
                TempData["SuccsesMessage"] = "Şifre Değişikliği Başarılı";
                _userService.EditPass(user, NewPassword);
                return RedirectToAction("UserView", "User");
            }
            else
            {
                TempData["ErrorMessage"] = "Mevcut Şifreniz Yanlış";
                return RedirectToAction("UserView", "User");
            }              
        }
    }
}
