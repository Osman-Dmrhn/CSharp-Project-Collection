using BlogAngApi.Db;
using BlogAngApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlogAngApi.Services
{
    
    public class CommentService:ICommentService
    {
        private readonly EfContext _efContext;

        public CommentService(EfContext efContext)
        {
            _efContext = efContext;
        }


        public bool AddComment(string content, Guid userId, Guid blogId)
        {
            // Yeni yorum nesnesi oluştur
            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                BlogId = blogId,
                CreatedAt = DateTime.Now // Yorum eklenme tarihi
            };

            // Veritabanına ekle
            _efContext.Yorumlar.Add(comment);
            _efContext.SaveChanges();
            return true;
        }
    }
  
}
