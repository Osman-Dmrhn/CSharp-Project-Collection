using BlogAngApi.Models;
using System.Runtime.Versioning;

namespace BlogAngApi.Model
{
    public class blogUpdModel
    {
        public Guid Id { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public DateTime DuzenlenmeTarihi { get; set; } = DateTime.Now;
    }
}
