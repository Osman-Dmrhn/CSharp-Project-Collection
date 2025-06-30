using BlogAngApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAngApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public CommentController(IUserService userService, ICommentService commentService)
        {
            _userService = userService;
            _commentService = commentService;
        }

        [HttpPost("commentadd")]
        public IActionResult Add_Comment([FromBody] CommentRequestModel request)
        { 
            var _currentGuid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(_currentGuid, out Guid userId);
            var user = _userService.GetUser(userId);

            if (_commentService.AddComment(request.Content, userId, request.BlogId))
            {
                return Ok(new { message = "Yorum başarıyla eklendi." });
            }

            return BadRequest(new { message = "Yorum eklenirken bir hata oluştu." });
        }
    }

    public class CommentRequestModel
    {
        public Guid BlogId { get; set; }
        public string Content { get; set; }
    }
}
