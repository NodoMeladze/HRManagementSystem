using HRManagement.Web.Constants;
using HRManagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRManagement.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index()
        {
            var token = HttpContext.Request.Cookies[CookieNames.AuthToken];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            return RedirectToAction("Index", "Employee");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = statusCode
            };

            if (statusCode.HasValue)
            {
                _logger.LogWarning("Error page accessed with status code: {StatusCode}", statusCode);
            }
            else
            {
                _logger.LogError("Error page accessed without status code");
            }

            return View(errorViewModel);
        }
    }
}