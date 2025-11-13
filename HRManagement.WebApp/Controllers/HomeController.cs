using HRManagement.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRManagement.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            // If authenticated, redirect to employee list
            if (AuthService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Employee");
            }

            // Otherwise show home page
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
