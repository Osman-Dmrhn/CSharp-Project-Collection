using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BlogApplication.Areas.AdminArea.ViewComponents
{
    public class LoggedInUserViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedInUserViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;

            return View("Default", userName); 
        }
    }
}
