using BlogAngApi.Model;
using BlogAngApi.Models;
using BlogAngApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAngApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;

        public BlogController(IBlogService blogService, IUserService userService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _userService = userService;
            _categoryService = categoryService;
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCategories();
            return Ok(categories);
        }

        [HttpGet("GetBlogById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetBlogById(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                return Unauthorized(new { message = "Kullanıcı bilgileri alınamadı." });
            }

            var blogs = _blogService.GetBlogById(userGuid);

         
            if (blogs == null || !blogs.Any())
            {
                return NotFound("Hiç blog bulunamadı.");
            }

            var blogList = blogs.Select(blog => new
            {
                blog.Id,
                blog.Baslik,
                blog.Aciklama,
                blog.Icerik,
                blog.ResimPath,
                blog.DuzenlenmeTarihi,
                blog.UserId,
                blog.Onay,
                
                Yazar = new
                {
                    blog.Yazar.Id,
                    blog.Yazar.Username
                },
                
                Yorumlar = blog.Yorumlar.Select(y => new
                {
                    y.Id,
                    y.Content,
                    y.CreatedAt
                }).ToList(),
                Kategoriler = blog.Kategoriler.Select(k => new
                {
                    k.Id,
                    k.KategoriAdi
                }).ToList()
            }).ToList();

            return Ok(blogList);
        }

        [HttpGet("GetBlog/{id}")]
        public IActionResult GetBlog(Guid id)
        {
            var blog = _blogService.GetBlog(id);
            return Ok(blog);
        }

        [HttpGet("getPhoto/{filename}")]
        public IActionResult GetPhoto(string filename)
        {
            // Dosyanın yolu
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", filename);

            // Eğer dosya mevcut değilse, 404 döndür
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            // Dosya içeriğini okuma
            var fileBytes = System.IO.File.ReadAllBytes(path);

            // Dosyanın uzantısını alarak MIME tipini belirleme
            var fileExtension = Path.GetExtension(filename).ToLowerInvariant();
            string mimeType = fileExtension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"  // Diğer türler için varsayılan MIME tipi
            };

            // Dosyayı döndür
            return File(fileBytes, mimeType);
        }

        [HttpPost("AddBlog")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddBlog([FromForm] BlogAddModel addblog, [FromForm] IFormFile photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is invalid.");
            }

            List<Category> categlist = new List<Category>();
            categlist.Add(_categoryService.GetcategoryByName(addblog.KategoriAdi));

            Blog blog = new Blog
            {
                Baslik = addblog.Baslik,
                Icerik = addblog.Icerik,
                Kategoriler = categlist
            };

            if (photo != null && photo.Length > 0)
            {
                var resimAdi = $"{Guid.NewGuid()}";
                var kayitYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", resimAdi);

                using (var stream = new FileStream(kayitYolu, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                blog.ResimPath = resimAdi;
            }
            else
            {
                blog.ResimPath = "../uploads/img1.1";
            }

            var _currentGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(_currentGuid, out Guid userId);
            var user = _userService.GetUser(userId);

            _blogService.addBlog(blog, user);
            return Ok(new { message = "Blog successfully added" });
        }


        [HttpPost("update/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateBlog([FromForm]blogUpdModel updblog)
        {
            if (_blogService.UpdateBlog(updblog))
            {
                return Ok(new { success = true, message = "Blog başarıyla silindi." });
            }

            return NotFound(new { success = false, message = "Blog bulunamadı." });
        }

        [HttpPost("delete/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteBlog(Guid id)
        {
            if (_blogService.removeBlog(id))
            {
                return Ok(new { success = true, message = "Blog başarıyla silindi." });
            }

            return NotFound(new { success = false, message = "Blog bulunamadı." });
        }
    }
}

