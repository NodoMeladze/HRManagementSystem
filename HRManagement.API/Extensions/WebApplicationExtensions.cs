using HRManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HRManagement.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                Log.Information("Database Initialization Started");

                Log.Information("Checking for pending database migrations...");
                await context.Database.MigrateAsync();
                Log.Information("Database migrations completed successfully");

                Log.Information("Checking if database seeding is needed...");
                await ApplicationDbContextSeed.SeedAsync(context, logger);
                Log.Information("Database seeding check completed");

                Log.Information("Database Initialization Completed");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during database initialization");
                throw;
            }
        }
    }
}
