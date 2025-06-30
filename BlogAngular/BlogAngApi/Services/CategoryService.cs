using BlogAngApi.Db;
using BlogAngApi.Models;

namespace BlogAngApi.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly EfContext _efContext;

        public CategoryService(EfContext efContext)
        {
            _efContext = efContext;
        }
        public List<Category> GetCategories()
        {
            var list = _efContext.Kategoriler.OrderBy(x=>x.KategoriAdi).ToList();
            return list;
        }

        public Category GetcategoryByName(string ad)
        {
            var getir = _efContext.Kategoriler.Where(x => x.KategoriAdi==ad).FirstOrDefault();
            return getir;
        }

        public void AddCategory(string kategoriAdi)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                KategoriAdi = kategoriAdi
            };

            _efContext.Kategoriler.Add(category);
            _efContext.SaveChanges();
        }
        public bool RemoveCategory(Guid id)
        {
            var silinecek =_efContext.Kategoriler.Find(id);

            if (silinecek is null)
                return false;
            _efContext.Kategoriler.Remove(silinecek);
            _efContext.SaveChanges();
            return true;
        }
         public bool UpdateCategory(Guid id,string kategoriAdi)
        {
            var guncellenecek=_efContext.Kategoriler.Find(id);
            if (guncellenecek != null)
            {
                guncellenecek.KategoriAdi =kategoriAdi;
                _efContext.Kategoriler.Update(guncellenecek);
                _efContext.SaveChanges();
                return true;
            }
            else 
                return false;
        }
    }
}
