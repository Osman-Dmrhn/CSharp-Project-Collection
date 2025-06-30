using BlogApplication.Models;

namespace BlogApplication.Services
{
    public interface ICommentService
    {
        bool AddComment(string content, Guid userId, Guid blogId);
    }
}
