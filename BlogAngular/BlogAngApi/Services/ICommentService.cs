using BlogAngApi.Models;

namespace BlogAngApi.Services
{
    public interface ICommentService
    {
        bool AddComment(string content, Guid userId, Guid blogId);
    }
}
