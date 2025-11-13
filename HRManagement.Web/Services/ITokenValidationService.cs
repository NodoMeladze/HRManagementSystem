namespace HRManagement.Web.Services
{
    public interface ITokenValidationService
    {
        Task<bool> ValidateTokenAsync(string token);
    }
}
