using HRManagement.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRManagement.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected IAuthService AuthService =>
            HttpContext.RequestServices.GetRequiredService<IAuthService>();

        protected ILogger Logger =>
            HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.CurrentUser = AuthService.GetCurrentUser();
            ViewBag.IsAuthenticated = AuthService.IsAuthenticated();

            base.OnActionExecuting(context);
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        protected void SetInfoMessage(string message)
        {
            TempData["InfoMessage"] = message;
        }
    }
}
