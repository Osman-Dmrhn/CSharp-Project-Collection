using System.ComponentModel.DataAnnotations;

namespace BlogAngApi.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string KategoriAdi { get; set; }

        public List <Blog> Bloglar { get; set; } = new List <Blog> ();
    }
}
