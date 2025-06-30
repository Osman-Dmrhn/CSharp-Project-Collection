using BlogAngApi.Model;
using BlogAngApi.Models;
using BlogAngApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAngApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IWebHostEnvironment environment, ILogger<UserController> logger)
        {
            _userService = userService;
            _environment = environment;
            _logger = logger;
        }

        [HttpGet("user-view")]
        public ActionResult<User> GetUserView()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized(new { message = "Kullanıcı bilgileri alınamadı." });
            }

            var user = _userService.GetUser(userGuid);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            return Ok(user);
        }

        // Fotoğraf yükleme
        [HttpPost("edit-photo")]
        public async Task<ActionResult> EditPhoto([FromForm] IFormFile photo)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userId, out Guid userGuid))
                {
                    return Unauthorized(new { message = "Kullanıcı bilgileri alınamadı." });
                }

                var user = _userService.GetUser(userGuid);
                if (photo == null || photo.Length == 0)
                {
                    return BadRequest(new { message = "Lütfen bir dosya seçin." });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if (!allowedExtensions.Contains(Path.GetExtension(photo.FileName).ToLower()))
                {
                    return BadRequest(new { message = "Sadece JPG, PNG veya GIF formatındaki dosyaları yükleyebilirsiniz." });
                }

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                user.proImage = "../uploads/" + fileName;
                _userService.EditPhoto(user);

                return Ok(new { message = "Fotoğraf başarıyla yüklendi!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fotoğraf yükleme sırasında bir hata oluştu.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Bir hata oluştu. Lütfen tekrar deneyin." });
            }
        }

        [HttpPost("edit-user")]
        public ActionResult EditUser([FromBody] User user)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized(new { message = "Kullanıcı bilgileri alınamadı." });
            }

            var existingUser = _userService.GetUser(userGuid);
            if (existingUser == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest(new { message = "Kullanıcı bilgileri geçersiz." });
            }

            user.Id = existingUser.Id;
            _userService.UpdateUser(user);

            return Ok(new { message = "Profil başarıyla güncellendi!" });
        }
 
        [HttpPost("edit-pass")]
        public ActionResult EditPassword([FromBody] ChangePasswordModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized(new { message = "Kullanıcı bilgileri alınamadı." });
            }

            var user = _userService.GetUser(userGuid);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                return BadRequest(new { message = "Tüm Alanları Doldurunuz" });
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new { message = "Yeni Şifre ve  Tekrar Birbirine Uymuyor" });
            }

            if (_userService.UserAuth(user.Email, model.CurrentPassword))
            {
                _userService.EditPass(user, model.NewPassword);
                return Ok(new { message = "Şifre Değişikliği Başarılı" });
            }

            return Unauthorized(new { message = "Mevcut Şifreniz Yanlış" });
        }
    }
}
