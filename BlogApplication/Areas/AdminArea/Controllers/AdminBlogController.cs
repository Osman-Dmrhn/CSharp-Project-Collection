using BlogApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AdminBlogController : Controller
    {
        private readonly IBlogService _blogService;

        public AdminBlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            var bloglar = _blogService.GetAll();
            return View(bloglar);
        }

         // Blog Onaylama
         [HttpPost]
         public IActionResult ApproveBlog(Guid id)
         {
            
             if (_blogService.acceptBlog(id))
             {
                 return Json(new { success = true, message = "Blog başarıyla onaylandı." });
             }

             return Json(new { success = false, message = "Blog bulunamadı." });
         }

         // Blog Silme
         [HttpPost]
         public IActionResult DeleteBlog(Guid id)
         {
             if (_blogService.removeBlog(id))
             {
                 return Json(new { success = true, message = "Blog başarıyla silindi." });
             }

             return Json(new { success = false, message = "Blog bulunamadı." });
         }
        [HttpGet]
        public IActionResult GetBlogDetails(Guid id)
        {
            var blog = _blogService.GetBlog(id);
            if (blog != null)
            {
                return Json(new
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
            return Json(new { success = false });
        }
    }
}
