using System.ComponentModel.DataAnnotations;

namespace BlogApplication.Models
{

    public class RegisterLoginViewModel
    {
        public User user { get; set; }
        public UserLogin login { get; set; }
    }
    public class UserLogin
    {
        [Required(ErrorMessage = "Email Girilmelidir")]
        [EmailAddress(ErrorMessage = "Geçersiz Mail Formatı")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Girilimelidir")]
        public string PasswordHash { get; set; }
    }
}
