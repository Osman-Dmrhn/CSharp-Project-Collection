using BlogAngApi.Models;
using BlogAngApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BlogAngApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogWithCategoryController : ControllerBase
    {
        private readonly IBlogService _blogservice;
        private readonly ICategoryService _categoryservice;

        public BlogWithCategoryController(IBlogService blogservice, ICategoryService categoryservice)
        {
            _blogservice = blogservice;
            _categoryservice = categoryservice;
        }


        [HttpGet("getBlogsWithCategories")]
        public IActionResult GetBlogsWithCategories()
        {
            var blogs = _blogservice.GetAcceptedBlogs();

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

        [HttpGet("error")]
        public IActionResult GetError()
        {
            return BadRequest("An error occurred.");
        }
    }
}
