using HRManagement.Web.Constants;
using HRManagement.Web.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void AddModelErrors<T>(ApiResponse<T> response)
        {
            if (response.Errors != null && response.Errors.Count != 0)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else if (!string.IsNullOrEmpty(response.Message))
            {
                ModelState.AddModelError(string.Empty, response.Message);
            }
        }

        protected bool IsAuthenticated()
        {
            var token = HttpContext.Request.Cookies[CookieNames.AuthToken];
            return !string.IsNullOrEmpty(token);
        }

        protected IActionResult RedirectToReturnUrlOrDefault(
            string? returnUrl,
            string defaultAction = "Index",
            string defaultController = "Employee")
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(defaultAction, defaultController);
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        protected void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }
    }
}