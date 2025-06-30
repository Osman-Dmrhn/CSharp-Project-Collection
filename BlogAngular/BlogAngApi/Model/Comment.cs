using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAngApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }
    }
}
