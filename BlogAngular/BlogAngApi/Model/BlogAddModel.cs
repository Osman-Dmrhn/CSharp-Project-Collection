using System.ComponentModel.DataAnnotations;

namespace BlogAngApi.Models
{
    public class BlogAddModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "Kullanıcı Adı En Az 5 Karakterden Oluşmalıdır")]
        public string Baslik { get; set; }

        [Required]
        [MinLength(250, ErrorMessage = "İçerik en 250 Karakterden Oluşmalıdır")]
        public string Icerik { get; set; }

        public String KategoriAdi { get; set; } 

    }
}
