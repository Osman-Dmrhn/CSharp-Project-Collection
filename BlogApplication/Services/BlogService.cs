using BlogApplication.Db;
using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection.Metadata;

namespace BlogApplication.Services
{
    public class BlogService:IBlogService
    {
        private readonly EfContext _efContext;

        public BlogService(EfContext efContext)
        {
            _efContext = efContext;
        }
        public List<Blog> GetAll()
        {
            var list = _efContext.Bloglar.Include(b => b.Yazar).Include(b => b.Kategoriler).ToList();
            return list;
        }


        public List<Blog> GetAcceptedBlogs(int page)
        {
            return _efContext.Bloglar.Include(b => b.Yazar)  .Include(b => b.Kategoriler)  .Where(x => x.Onay == true)  .Skip((page - 1) * 6).Take(6).ToList();  
        }

        public int GetTotalBlogCount()
        {
            
            return _efContext.Bloglar
                             .Where(x => x.Onay == true)  
                             .Count();  
        }

        public List<Blog> GetBlogsById(Guid id)
        {
            var list = _efContext.Bloglar.Include(b => b.Yazar).Include(b => b.Kategoriler).Where(x => x.UserId == id).ToList();
            return list;
        }

        public Blog GetBlog(Guid id)
        {
            var getirilecek= _efContext.Bloglar.Include(b => b.Yazar).Include(z=>z.Kategoriler).Include(c=>c.Yorumlar).ThenInclude(c => c.User).AsNoTracking().FirstOrDefault(x => x.Id == id);
            return getirilecek;
        }
        public bool acceptBlog(Guid id)
        {
            var guncellenecek = _efContext.Bloglar.Find(id);
            if (guncellenecek is not null)
            {
                guncellenecek.Onay = true;
                _efContext.Bloglar.Update(guncellenecek);
                _efContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void addBlog(Blog blog,User user)
        {
            blog.UserId = user.Id;
            blog.Id = Guid.NewGuid(); // Yeni bir GUID oluştur
            if (blog.Icerik.Length > 250)
                blog.Aciklama = blog.Icerik.Substring(0, 250) + "...";
            else
                blog.Aciklama = blog.Icerik.Substring(0, 100) + "...";
            blog.DuzenlenmeTarihi = DateTime.Now; // Güncel tarih
            blog.Onay = false; // Onay durumu başlangıçta false olabilir

            var existingCategories = blog.Kategoriler
                .Select(c => _efContext.Kategoriler.FirstOrDefault(k => k.Id == c.Id))
                .Where(c => c != null)
                .ToList();

            // Kategorileri eklerken tekrar yaratmayı önler
            blog.Kategoriler.Clear();
            foreach (var kategori in existingCategories)
            {
                _efContext.Attach(kategori);  // Mevcut kategoriyi bağlar
                blog.Kategoriler.Add(kategori);
            }

            // Blog ekleme işlemi
            _efContext.Bloglar.Add(blog);
            _efContext.SaveChanges();
        }
        public bool removeBlog(Guid id)
        {
            var silinecek= _efContext.Bloglar.Find(id);
            if (silinecek is null)
                return false;
            _efContext.Remove(silinecek);
            _efContext.SaveChanges();
            return true;
        }

        public bool UpdateBlog(BlogUpdModel blog)
        {
            var guncellenecek = _efContext.Bloglar.Find(blog.id);
            string aciklama;
            if (blog.Icerik.Length > 250)
                aciklama = blog.Icerik.Substring(0, 250) + "...";
            else
                aciklama = blog.Icerik.Substring(0, 100) + "...";
            if (guncellenecek is not null)
            {
                guncellenecek.Icerik=blog.Icerik;
                guncellenecek.Aciklama = aciklama;
                guncellenecek.Baslik = blog.Baslik;
                guncellenecek.Onay = false;
                guncellenecek.DuzenlenmeTarihi = DateTime.Now;
                _efContext.Bloglar.Update(guncellenecek);
                _efContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
