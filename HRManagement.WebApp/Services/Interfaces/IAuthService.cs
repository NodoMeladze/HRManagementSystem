using HRManagement.WebApp.Models.ApiModels.Auth;

namespace HRManagement.WebApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> LoginAsync(LoginRequest request);
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request);
        Task LogoutAsync();
        bool IsAuthenticated();
        string? GetToken();
        UserDto? GetCurrentUser();
    }
}
