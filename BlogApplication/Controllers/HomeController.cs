using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogservice;
        private readonly ICategoryService _categoryservice;

        public HomeController(ILogger<HomeController> logger,IBlogService blogservice, ICategoryService categoryservice)
        {
            _logger = logger;
            _blogservice = blogservice;
            _categoryservice = categoryservice;
        }

        public IActionResult Index(int page=1)
        {
            int totalItems = _blogservice.GetTotalBlogCount();
            int totalPages = (int)Math.Ceiling((double)totalItems / 6);

            var blogs = _blogservice.GetAcceptedBlogs(page);
            var categ=_categoryservice.GetCategories();

            var blogcateg = new BlogWithCategory
            {
                Blogs = blogs,
                Kategoriler = categ,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems
            };
            return View(blogcateg);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
