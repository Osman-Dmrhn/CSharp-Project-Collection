using BlogApplication.Models;

namespace BlogApplication.Services
{
    public interface IBlogService
    {
        List<Blog> GetAll();
        List<Blog> GetAcceptedBlogs(int page);

        public int GetTotalBlogCount();

        List<Blog> GetBlogsById(Guid id);
        void addBlog(Blog blog,User user);
        Blog GetBlog(Guid id);
        bool removeBlog(Guid id);
        bool acceptBlog(Guid id);
        bool UpdateBlog(BlogUpdModel blog);
    }
}
