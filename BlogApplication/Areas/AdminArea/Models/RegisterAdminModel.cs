using System.ComponentModel.DataAnnotations;

namespace BlogApplication.Areas.AdminArea.Models
{
    public class RegisterAdminModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı gereklidir.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı en az 3, en fazla 50 karakter olmalıdır.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Sifre { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur.")]
        public string Rol { get; set; }
    }
}
