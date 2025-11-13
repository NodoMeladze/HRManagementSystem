using HRManagement.Web.Middleware;
using HRManagement.Web.Services;
using Serilog;

namespace HRManagement.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureMiddlewareAsync(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
                };
            });

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<TokenValidationMiddleware>();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapGet("/health", () => Results.Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                environment = app.Environment.EnvironmentName
            }));

            return app;
        }

        public static async Task VerifyApiConnectionAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var apiClient = scope.ServiceProvider.GetRequiredService<ApiClient>();

            try
            {
                Log.Information("Verifying API connection...");
                var response = await apiClient.GetAsync<object>("/api/position");

                if (response.Success)
                {
                    Log.Information("API connection verified successfully");
                }
                else
                {
                    Log.Warning("API is reachable but returned error: {Message}", response.Message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to connect to API. Please ensure the API is running.");
            }
        }
    }
}