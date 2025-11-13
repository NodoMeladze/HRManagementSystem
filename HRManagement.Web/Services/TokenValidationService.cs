namespace HRManagement.Web.Services
{
    public class TokenValidationService(
        ApiClient apiClient,
        ILogger<TokenValidationService> logger) : ITokenValidationService
    {
        private readonly ApiClient _apiClient = apiClient;
        private readonly ILogger<TokenValidationService> _logger = logger;

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var response = await _apiClient.GetAsync<object>("/api/auth/me");
                return response.Success;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return false;
            }
        }
    }
}
