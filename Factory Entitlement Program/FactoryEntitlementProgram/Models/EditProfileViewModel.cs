using System.ComponentModel.DataAnnotations;

namespace FactoryEntitlementProgram.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }
}
