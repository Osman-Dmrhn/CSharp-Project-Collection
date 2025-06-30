
using BlogAngApi.Areas.AdminArea.Models;
using BlogAngApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BlogAngApi.Db
{
    public class EfContext:DbContext
    {

        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Blog ile Category arasındaki ilişkiyi belirtelim
            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Blog)
                .WithMany()
                .HasForeignKey(pc => pc.BlogId);

            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Category)
                .WithMany()
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<Comment>()
            .HasOne(c => c.User) // Comment ile User arasındaki ilişki
            .WithMany(u => u.Comments) // User ile Comments arasındaki ilişki
            .HasForeignKey(c => c.UserId) // Yorumun UserId ile ilişkisini kur
            .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse bile, yorum silinmez

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Yazar) // Blog ile Yazar (User) arasındaki ilişki
                .WithMany(u => u.Bloglar) // User ile Blogs arasındaki ilişki
                .HasForeignKey(b => b.UserId) // Blogun UserId ile ilişkisini kur
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinse bile, blog silinmez
        }

        public DbSet<User> Kullanıcılar { get; set; }
        public DbSet<Blog> Bloglar { get; set; }
        public DbSet<Comment> Yorumlar { get; set; }
        public DbSet<Category> Kategoriler { get; set; }
        public DbSet<PostCategory> PostKategorileri { get; set; }

        public DbSet<Admin> Adminler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=OSMAN\\MSSQLSERVERD;Database=blog_ang;User Id=osman;Password=123456;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
