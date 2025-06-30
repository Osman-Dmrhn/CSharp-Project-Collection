using BlogApplication.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;
using System.Web.Mvc;

namespace BlogApplication.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlContent UserNameDisplay(this IHtmlHelper htmlHelper)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;

            if (httpContext.User.Identity.IsAuthenticated)
            {
                return new HtmlString($"<span class='username' style='color: white;'>Hoş geldin, {httpContext.User.Identity.Name}!</span>");
            }
            else
            {
                return new HtmlString("<span class='username' style='color: white;'>Bize Katılın!</span>");
            }
        }
    }
}

