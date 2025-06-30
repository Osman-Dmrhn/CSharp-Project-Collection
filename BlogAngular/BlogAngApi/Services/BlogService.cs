using BlogAngApi.Db;
using BlogAngApi.Model;
using BlogAngApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection.Metadata;

namespace BlogAngApi.Services
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

        public List<Blog> GetBlogById(Guid id)
        {
            var list= _efContext.Bloglar.Include(b => b.Yazar).Include(z => z.Kategoriler).Include(c => c.Yorumlar).ThenInclude(c => c.User).Where(c=>c.UserId==id).AsNoTracking().ToList();
            return list;
        }

        public List<Blog> GetAcceptedBlogs()
        {
            var list = _efContext.Bloglar.Include(b => b.Yazar).Include(b => b.Kategoriler).Where(x => x.Onay == true).ToList();
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
            blog.Id = Guid.NewGuid(); 
            if (blog.Icerik.Length > 250)
                blog.Aciklama = blog.Icerik.Substring(0, 250) + "...";
            else
                blog.Aciklama = blog.Icerik.Substring(0, 100) + "...";
            blog.DuzenlenmeTarihi = DateTime.Now; 
            blog.Onay = false; 

            var existingCategories = blog.Kategoriler
                .Select(c => _efContext.Kategoriler.FirstOrDefault(k => k.Id == c.Id))
                .Where(c => c != null)
                .ToList();

            
            blog.Kategoriler.Clear();
            foreach (var kategori in existingCategories)
            {
                _efContext.Attach(kategori);  
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

        public bool UpdateBlog(blogUpdModel blog)
        {
            var guncellenecek = _efContext.Bloglar.Find(blog.Id);
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
                guncellenecek.DuzenlenmeTarihi = blog.DuzenlenmeTarihi;
                guncellenecek.Onay = false;
                _efContext.Bloglar.Update(guncellenecek);
                _efContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
