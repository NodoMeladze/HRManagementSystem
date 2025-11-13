using HRManagement.Web.Constants;
using HRManagement.Web.Services;

namespace HRManagement.Web.Middleware
{
    public class TokenValidationMiddleware(
        RequestDelegate next,
        ILogger<TokenValidationMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<TokenValidationMiddleware> _logger = logger;

        public async Task InvokeAsync(
            HttpContext context,
            ITokenValidationService tokenValidationService)
        {
            var token = context.Request.Cookies[CookieNames.AuthToken];

            if (!string.IsNullOrEmpty(token))
            {
                var isValid = await tokenValidationService.ValidateTokenAsync(token);

                if (!isValid)
                {
                    _logger.LogWarning("Invalid or expired token detected. Clearing cookies.");

                    context.Response.Cookies.Delete(CookieNames.AuthToken);
                    context.Response.Cookies.Delete(CookieNames.UserEmail);
                    context.Response.Cookies.Delete(CookieNames.UserName);
                    context.Response.Cookies.Delete(CookieNames.UserId);

                    if (IsAjaxRequest(context.Request))
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            success = false,
                            message = "თქვენი სესია ამოიწურა. გთხოვთ თავიდან შეხვიდეთ სისტემაში"
                        });
                        return;
                    }

                    if (context.Request.Path.StartsWithSegments("/Employee") ||
                        context.Request.Path.StartsWithSegments("/Position"))
                    {
                        context.Response.Redirect($"/Account/Login?returnUrl={Uri.EscapeDataString(context.Request.Path)}");
                        return;
                    }
                }
            }

            await _next(context);
        }

        private static bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers.XRequestedWith == "XMLHttpRequest" ||
                   request.Headers.Accept.ToString().Contains("application/json");
        }
    }
}