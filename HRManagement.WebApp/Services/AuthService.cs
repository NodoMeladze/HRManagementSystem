using HRManagement.WebApp.Models.ApiModels;
using HRManagement.WebApp.Models.ApiModels.Auth;
using HRManagement.WebApp.Services.Interfaces;
using Newtonsoft.Json;
namespace HRManagement.WebApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApiService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        private const string TokenKey = "AuthToken";
        private const string UserKey = "CurrentUser";
        private const string TokenExpiryKey = "TokenExpiry";

        public AuthService(
            IApiService apiService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _apiService.PostAsync<LoginRequest, ApiResponse<AuthResponse>>(
                    "auth/login",
                    request);

                if (response?.Success == true && response.Data != null)
                {
                    StoreAuthData(response.Data);
                    _apiService.SetAuthToken(response.Data.Token);

                    _logger.LogInformation("User logged in successfully: {Email}", response.Data.User.Email);
                    return (true, response.Message);
                }

                var errorMessage = response?.Message ?? "ავტორიზაცია ვერ მოხერხდა";
                _logger.LogWarning("Login failed: {Message}", errorMessage);
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return (false, "სერვერთან კავშირი ვერ დამყარდა");
            }
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var response = await _apiService.PostAsync<RegisterRequest, ApiResponse<AuthResponse>>(
                    "auth/register",
                    request);

                if (response?.Success == true && response.Data != null)
                {
                    StoreAuthData(response.Data);
                    _apiService.SetAuthToken(response.Data.Token);

                    _logger.LogInformation("User registered successfully: {Email}", response.Data.User.Email);
                    return (true, response.Message);
                }

                var errorMessage = response?.Message ?? "რეგისტრაცია ვერ მოხერხდა";
                _logger.LogWarning("Registration failed: {Message}", errorMessage);
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return (false, "სერვერთან კავშირი ვერ დამყარდა");
            }
        }

        public Task LogoutAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(TokenKey);
                session.Remove(UserKey);
                session.Remove(TokenExpiryKey);
            }

            _apiService.ClearAuthToken();
            _logger.LogInformation("User logged out");

            return Task.CompletedTask;
        }

        public bool IsAuthenticated()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var expiryString = _httpContextAccessor.HttpContext?.Session.GetString(TokenExpiryKey);
            if (string.IsNullOrEmpty(expiryString))
            {
                return false;
            }

            if (DateTime.TryParse(expiryString, out var expiry))
            {
                if (expiry <= DateTime.UtcNow)
                {
                    _logger.LogInformation("Token expired, clearing session");
                    LogoutAsync().Wait();
                    return false;
                }
                return true;
            }

            return false;
        }

        public string? GetToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.SetAuthToken(token);
            }
            return token;
        }

        public UserDto? GetCurrentUser()
        {
            var userJson = _httpContextAccessor.HttpContext?.Session.GetString(UserKey);
            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<UserDto>(userJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize user from session");
                return null;
            }
        }

        private void StoreAuthData(AuthResponse authResponse)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available");
            }

            session.SetString(TokenKey, authResponse.Token);
            session.SetString(UserKey, JsonConvert.SerializeObject(authResponse.User));
            session.SetString(TokenExpiryKey, authResponse.ExpiresAt.ToString("O"));

            _logger.LogDebug("Auth data stored in session for user: {Email}", authResponse.User.Email);
        }
    }

}
