using System.ComponentModel.DataAnnotations;

namespace BlogAngApi.Models
{

    public class RegisterLoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Geçersiz Mail Formatı")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre Girilimelidir")]
        public string PasswordHash { get; set; }
    }
}
