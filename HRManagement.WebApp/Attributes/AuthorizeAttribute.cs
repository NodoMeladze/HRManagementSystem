using HRManagement.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRManagement.WebApp.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authService = context.HttpContext.RequestServices
                .GetRequiredService<IAuthService>();

            if (!authService.IsAuthenticated())
            {
                // Redirect to login page
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    new { returnUrl = context.HttpContext.Request.Path });
            }
        }
    }
}
