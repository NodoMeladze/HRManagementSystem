using System.Net;

namespace HRManagement.Web.Middleware
{
    public class GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An unhandled exception occurred: {Message}. Path: {Path}",
                    ex.Message,
                    context.Request.Path);

                await HandleExceptionAsync(context);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Redirect($"/Home/Error?statusCode={context.Response.StatusCode}");
            return Task.CompletedTask;
        }
    }
}