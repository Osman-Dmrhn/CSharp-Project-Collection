namespace BlogApplication.Models
{
    public class BlogWithCategory
    {
        public List<Blog> Blogs { get; set; }  
        public List<Category> Kategoriler { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

    }
}
