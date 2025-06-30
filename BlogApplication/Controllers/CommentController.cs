using BlogApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApplication.Controllers
{
    [Authorize(AuthenticationSchemes = "user")]

    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        public CommentController(IUserService userService,ICommentService commentService)
        {
            _userService = userService; 
            _commentService = commentService;
        }
        [HttpPost]
        public IActionResult Add_Comment(Guid blogId, string content)
        {
            var _currentGuid = (User.FindFirstValue(ClaimTypes.NameIdentifier));
            Guid.TryParse(_currentGuid, out Guid userId);
            var user = _userService.GetUser(userId);

            if(_commentService.AddComment(content, userId , blogId))
            {
                TempData["SuccessMessage"] = "Yorum başarıyla eklendi.";
            }
            return RedirectToAction("BlogIndex", "Blog", new { id = blogId });
        }
    }
}
