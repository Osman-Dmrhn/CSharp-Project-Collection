using BlogAngApi.Model;
using BlogAngApi.Models;

namespace BlogAngApi.Services
{
    public interface IBlogService
    {
        List<Blog> GetAll();
        List<Blog> GetAcceptedBlogs();

        List<Blog> GetBlogById(Guid id);
        void addBlog(Blog blog,User user);
        Blog GetBlog(Guid id);
        bool removeBlog(Guid id);
        bool acceptBlog(Guid id);
        bool UpdateBlog(blogUpdModel blog);
    }
}
