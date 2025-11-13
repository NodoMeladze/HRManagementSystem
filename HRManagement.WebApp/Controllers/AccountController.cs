using HRManagement.WebApp.Models.ApiModels.Auth;
using HRManagement.WebApp.Models.ViewModels.Auth;
using HRManagement.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.WebApp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthService authService,
            ILogger<AccountController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If already authenticated, redirect to employee list
            if (_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Employee");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new LoginRequest
            {
                Username = model.Username,
                Password = model.Password
            };

            var (success, message) = await _authService.LoginAsync(request);

            if (success)
            {
                _logger.LogInformation("User logged in: {Username}", model.Username);
                SetSuccessMessage(message);

                // Redirect to return URL or employee list
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Employee");
            }

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            // If already authenticated, redirect to employee list
            if (_authService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Employee");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var request = new RegisterRequest
            {
                PersonalNumber = model.PersonalNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                DateOfBirth = DateOnly.FromDateTime(model.DateOfBirth).ToString("yyyy-MM-dd"),
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword
            };

            var (success, message) = await _authService.RegisterAsync(request);

            if (success)
            {
                _logger.LogInformation("User registered: {Email}", model.Email);
                SetSuccessMessage(message);
                return RedirectToAction("Index", "Employee");
            }

            // If user already exists, redirect to login
            if (message.Contains("უკვე არსებობს"))
            {
                SetInfoMessage(message);
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            SetSuccessMessage("წარმატებით გამოხვედით სისტემიდან");
            return RedirectToAction("Index", "Home");
        }
    }
}
