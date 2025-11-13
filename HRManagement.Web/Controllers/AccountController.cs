using AutoMapper;
using HRManagement.Web.Constants;
using HRManagement.Web.Models.API;
using HRManagement.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Web.Controllers
{
    public class AccountController(
        ApiClient apiClient,
        IMapper mapper,
        ILogger<AccountController> logger) : BaseController
    {
        private readonly ApiClient _apiClient = apiClient;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AccountController> _logger = logger;

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            if (IsAuthenticated())
            {
                return RedirectToAction("Index", "Employee");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registerDto = _mapper.Map<object>(model);
            var response = await _apiClient.PostAsync<AuthResponse>(ApiEndpoints.Register, registerDto);

            if (response.Success && response.Data != null)
            {
                StoreAuthToken(response.Data, rememberMe: false);

                _logger.LogInformation("User registered successfully: {Email}", response.Data.User.Email);
                SetSuccessMessage(ValidationMessages.RegistrationSuccess);

                return RedirectToReturnUrlOrDefault(returnUrl);
            }

            AddModelErrors(response);
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (IsAuthenticated())
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

            var loginDto = _mapper.Map<object>(model);
            var response = await _apiClient.PostAsync<AuthResponse>(ApiEndpoints.Login, loginDto);

            if (response.Success && response.Data != null)
            {
                StoreAuthToken(response.Data, rememberMe: model.RememberMe);

                _logger.LogInformation("User logged in successfully: {Email}", response.Data.User.Email);
                SetSuccessMessage(ValidationMessages.LoginSuccess);

                return RedirectToReturnUrlOrDefault(returnUrl);
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            ClearAuthCookies();

            _logger.LogInformation("User logged out");
            SetSuccessMessage(ValidationMessages.LogoutSuccess);
            return RedirectToAction(nameof(Login));
        }

        #region Helper Methods

        private void StoreAuthToken(AuthResponse authResponse, bool rememberMe)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = HttpContext.Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = rememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(24)
            };

            Response.Cookies.Append(CookieNames.AuthToken, authResponse.Token, cookieOptions);

            var userInfoOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = HttpContext.Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = cookieOptions.Expires
            };

            Response.Cookies.Append(CookieNames.UserEmail, authResponse.User.Email, userInfoOptions);
            Response.Cookies.Append(CookieNames.UserName, $"{authResponse.User.FirstName} {authResponse.User.LastName}", userInfoOptions);
            Response.Cookies.Append(CookieNames.UserId, authResponse.User.Id.ToString(), userInfoOptions);
        }

        private void ClearAuthCookies()
        {
            Response.Cookies.Delete(CookieNames.AuthToken);
            Response.Cookies.Delete(CookieNames.UserEmail);
            Response.Cookies.Delete(CookieNames.UserName);
            Response.Cookies.Delete(CookieNames.UserId);
        }

        #endregion
    }
}