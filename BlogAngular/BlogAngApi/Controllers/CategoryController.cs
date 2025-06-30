using BlogAngApi.Model;
using BlogAngApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngApi.Controllers
{
    [Route("api/admin/categories")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCategories();
            return Ok(categories);
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody] CategoryDto categoryDto)
        {
            _categoryService.AddCategory(categoryDto.KategoriAdi);
            return Ok(new { success = true, message = "Kategori başarıyla eklendi!" });
        }

        [HttpPut("{id}")]
        public IActionResult EditCategory(Guid id, [FromBody] CategoryDto categoryDto)
        {
            if (_categoryService.UpdateCategory(id, categoryDto.KategoriAdi))
            {
                return Ok(new { success = true, message = "Kategori güncellendi!" });
            }
            return NotFound(new { success = false, message = "Kategori bulunamadı!" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(Guid id)
        {
            if (_categoryService.RemoveCategory(id))
            {
                return Ok(new { success = true, message = "Kategori silindi!" });
            }
            return NotFound(new { success = false, message = "Kategori bulunamadı!" });
        }
    }
}  
