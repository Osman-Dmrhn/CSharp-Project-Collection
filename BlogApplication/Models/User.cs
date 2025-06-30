using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogApplication.Models
{
    public class User
    {
        [Key] // Birincil anahtar
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Kullanıcı Adı Girilimelidir")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı Adı En Az 3  Karakter En Fazla 50 Karakterden Oluşmalı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email Girilmelidir")]
        [EmailAddress(ErrorMessage = "Geçersiz Mail Formatı")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Girilimelidir")]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string proImage { get; set; }

        // Navigation Properties
        public List<Blog> Bloglar { get; set; } = new List<Blog>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
