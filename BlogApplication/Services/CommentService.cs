using BlogApplication.Db;
using BlogApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BlogApplication.Services
{
    [Authorize(AuthenticationSchemes = "user")]
    public class CommentService:ICommentService
    {
        private readonly EfContext _efContext;

        public CommentService(EfContext efContext)
        {
            _efContext = new EfContext();
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
