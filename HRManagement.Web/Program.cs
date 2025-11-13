using HRManagement.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilogLogging();
builder.Host.UseSerilog();

builder.Services.AddWebServices();
builder.Services.AddApiClient(builder.Configuration);
builder.Services.AddSecurityServices();

var app = builder.Build();

app.ConfigureMiddlewareAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

await app.VerifyApiConnectionAsync();

Log.Information("=".PadRight(60, '='));
Log.Information("HR Management Web Application Starting");
Log.Information("Environment: {Environment}", app.Environment.EnvironmentName);
Log.Information("API Base URL: {ApiUrl}", builder.Configuration["ApiSettings:BaseUrl"]);
Log.Information("=".PadRight(60, '='));

try
{
    app.Run();
    Log.Information("HR Management Web Application stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "HR Management Web Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}