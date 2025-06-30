using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult categoryAdd()
        {
            return View();
        }

        public ViewResult categoryRemove()
        {
            return View();
        }

        public ViewResult categoryUpdate() {
            return View();
        }
    }
}
