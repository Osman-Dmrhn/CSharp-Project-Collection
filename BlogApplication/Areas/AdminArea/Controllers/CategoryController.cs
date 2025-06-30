using BlogApplication.Models;
using BlogApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryservice;
        public CategoryController(ICategoryService categoryService) {
            _categoryservice = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var kategoriler=_categoryservice.GetCategories();
            return View(kategoriler);
        }

        [HttpPost]
        public IActionResult AddCategory(string kategoriAdi)
        {
            _categoryservice.AddCategory(kategoriAdi);
            TempData["SuccessMessage"] = "Kategori  eklendi!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditCategory(Guid id, string kategoriAdi)
        {
            if (_categoryservice.UpdateCategory(id,kategoriAdi))
            {
                TempData["SuccessMessage"] = "Kategori  guncellendi!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Kategori Bulunamadı";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteCategory(Guid id)
        {
            if (_categoryservice.RemoveCategory(id))
            {
                TempData["SuccessMessage"] = "Kategori  silindi!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Kategori Bulunamadı";
            return RedirectToAction("Index"); ;
        }
    }
}
