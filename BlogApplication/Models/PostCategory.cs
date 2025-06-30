using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApplication.Models
{
    public class PostCategory
    {
        [Key] // Primary Key tanımı
        public Guid Id { get; set; }

        [Required]
        public Guid BlogId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
