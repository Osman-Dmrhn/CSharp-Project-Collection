using BlogApplication.Models;

namespace BlogApplication.Services
{
    public interface ICategoryService
    {
        List<Category> GetCategories();

        Category GetcategoryByName(string ad);
        void AddCategory(string kategoriAdi);
        bool RemoveCategory(Guid id);
        bool UpdateCategory(Guid id, string kategoriAdi);
    }
}
