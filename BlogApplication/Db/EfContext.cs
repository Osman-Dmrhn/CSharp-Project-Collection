using BlogApplication.Areas.AdminArea.Models;
using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogApplication.Db
{


    public class EfContext :DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Blog)
                .WithMany()
                .HasForeignKey(pc => pc.BlogId);

            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.User) 
            .WithMany(u => u.Comments) 
            .HasForeignKey(c => c.UserId) 
            .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Yazar) 
                .WithMany(u => u.Bloglar) 
                .HasForeignKey(b => b.UserId) 
                .OnDelete(DeleteBehavior.Restrict); 
        }

        public DbSet<User> Kullanıcılar { get; set; }
        public DbSet<Blog> Bloglar { get; set; }
        public DbSet<Comment> Yorumlar { get; set; }
        public DbSet<Category> Kategoriler { get; set; }
        public DbSet<PostCategory> PostKategorileri { get; set; }

        public DbSet<Admin> Adminler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=OSMAN\\MSSQLSERVERD;Database=blog_data;User Id=osman;Password=123456;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
