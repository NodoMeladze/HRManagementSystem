using HRManagement.Web.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRManagement.Web.Filters
{
    public class AuthorizeSessionAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Cookies[CookieNames.AuthToken];

            if (string.IsNullOrEmpty(token))
            {
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    new { returnUrl });
            }
        }
    }
}