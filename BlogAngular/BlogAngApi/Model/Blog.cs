using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAngApi.Models
{
    public class Blog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Kullanıcı Adı En Az 5 Karakterden Oluşmalıdır")]
        public string Baslik { get; set; }

        public string Aciklama { get; set; }
        [Required]
        [MinLength(250, ErrorMessage = "İçerik en 250 Karakterden Oluşmalıdır")]
        public string Icerik {  get; set; }

        public string ResimPath { get; set; }

        public DateTime DuzenlenmeTarihi { get; set; }

        public Guid UserId { get; set; }

        public bool Onay { get; set; }

        [ForeignKey("UserId")]


        public User Yazar { get; set; }

        public List<Comment> Yorumlar { get; set; } = new List<Comment>();
        public List<Category> Kategoriler { get; set; } = new List<Category>();
    }
}
