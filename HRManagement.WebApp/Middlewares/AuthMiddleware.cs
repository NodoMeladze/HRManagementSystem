using HRManagement.WebApp.Services.Interfaces;

namespace HRManagement.Web.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(
            RequestDelegate next,
            ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            // Get token from session and set it for API calls
            var token = authService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                // Validate if token is still valid
                if (!authService.IsAuthenticated())
                {
                    _logger.LogInformation("Token expired, redirecting to login");

                    // Only redirect if not already going to login/register
                    var path = context.Request.Path.Value?.ToLower() ?? "";
                    if (!path.Contains("/account/login") &&
                        !path.Contains("/account/register") &&
                        !path.Contains("/home"))
                    {
                        context.Response.Redirect("/Account/Login");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}