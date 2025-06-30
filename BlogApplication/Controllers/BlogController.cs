using BlogApplication.Db;
using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApplication.Controllers
{
    
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        public BlogController(IBlogService blogservice,IUserService userservice,ICategoryService categoryservice) {
            _blogService = blogservice;
            _userService = userservice;
            _categoryService= categoryservice; 
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "user")]
        public IActionResult BlogView()
        {
            var categories = _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "KategoriAdi");
            return View();
        }

        public IActionResult BlogIndex(Guid id)
        {
            var blog =_blogService.GetBlog(id);
            return View(blog);
        }

        [Authorize(AuthenticationSchemes = "user")]
        public IActionResult GetBlogById()
        {
            var _currentGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(_currentGuid, out Guid userId);
            var blogs =_blogService.GetBlogsById(userId);
            return View(blogs);
        }

        [Authorize(AuthenticationSchemes = "user")]
        public IActionResult addBlog(BlogAddModel addblog, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                List<Category> categlist=new List<Category>();
                categlist.Add(_categoryService.GetcategoryByName(addblog.KategoriAdi));
                Blog blog = new()
                {
                    Baslik = addblog.Baslik,
                    Icerik = addblog.Icerik,
                    Kategoriler =categlist
                    
                };
                // Resim Yükleme İşlemi
                if (photo != null &&    photo.Length > 0)
                {
                    var resimAdi = $"{Guid.NewGuid()}_{photo.FileName}";
                    var kayitYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", resimAdi);

                    using (var stream = new FileStream(kayitYolu, FileMode.Create))
                    {
                        photo.CopyTo(stream);
                    }

                    blog.ResimPath = "../uploads/"+resimAdi;
                }
                else
                {
                    blog.ResimPath = "../uploads/img1.1";
                }
                var _currentGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(_currentGuid, out Guid userId);
                var user=_userService.GetUser(userId);
               
                _blogService.addBlog(blog,user);
                TempData["SuccessMessage"] = "Blog Başarıyla Gönderildi";
                return RedirectToAction("BlogView"); // Başarılı eklemeden sonra yönlendirme
            }

            return View("BlogView"); // Hata durumunda yeniden form
        }

        [Authorize(AuthenticationSchemes = "user")]
        public IActionResult removeBlog(Guid id)
        {
            _blogService.removeBlog(id);
            return RedirectToAction("GetBlogById");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "user")]
        public IActionResult editBlog(Guid id)
        {
            var blog = _blogService.GetBlog(id);
            var blogUpdModel = new BlogUpdModel
            {
                id = blog.Id,
                Baslik = blog.Baslik,
                Icerik = blog.Icerik
            };
            return View(blogUpdModel);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "user")]
        public IActionResult editBlog(BlogUpdModel blog)
        {
            _blogService.UpdateBlog(blog);
            return RedirectToAction("GetBlogById");
        }
    }
}
