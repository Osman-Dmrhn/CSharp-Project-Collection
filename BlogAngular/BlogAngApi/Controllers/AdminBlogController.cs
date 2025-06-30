using BlogAngApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngApi.Controllers
{
    [Route("api/admin/blogs")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdminBlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public AdminBlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("getBlogs")]
        public IActionResult GetAllBlogs()
        {
            var blogs = _blogService.GetAll();

            // Eğer blog bulunamazsa 404 döndür
            if (blogs == null || !blogs.Any())
            {
                return NotFound("Hiç blog bulunamadı.");
            }

            // Blogları düzleştirilmiş formatta serileştiriyoruz
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
                // Yazar bilgilerini düzleştiriyoruz
                Yazar = new
                {
                    blog.Yazar.Id,
                    blog.Yazar.Username
                },
                // Yorumları düzleştiriyoruz
                Yorumlar = blog.Yorumlar.Select(y => new
                {
                    y.Id,
                    y.Content,
                    y.CreatedAt
                }).ToList(),
                // Kategorileri düzleştiriyoruz
                Kategoriler = blog.Kategoriler.Select(k => new
                {
                    k.Id,
                    k.KategoriAdi
                }).ToList()
            }).ToList();

            // Tüm blogları döndürüyoruz
            return Ok(blogList);
        }

        [HttpPost("approve/{id}")]
        public IActionResult ApproveBlog( Guid id)
        {
            if (_blogService.acceptBlog(id))
            {
                return Ok(new { success = true, message = "Blog başarıyla onaylandı." });
            }

            return NotFound(new { success = false, message = "Blog bulunamadı." });
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteBlog(Guid id)
        {
            if (_blogService.removeBlog(id))
            {
                return Ok(new { success = true, message = "Blog başarıyla silindi." });
            }

            return NotFound(new { success = false, message = "Blog bulunamadı." });
        }

        [HttpGet("{id}")]
        public IActionResult GetBlogDetails(Guid id)
        {
            var blog = _blogService.GetBlog(id);
            if (blog != null)
            {
                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        baslik = blog.Baslik,
                        icerik = blog.Icerik,
                        yazar = blog.Yazar.Username
                    }
                });
            }
            return NotFound(new { success = false, message = "Blog bulunamadı." });
        }
    }

}
