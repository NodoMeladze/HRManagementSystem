using HRManagement.Domain.Entities;

namespace HRManagement.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        Guid? ValidateToken(string token);
    }
}
