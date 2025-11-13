using HRManagement.Application.DTOs.Auth;
using HRManagement.Application.DTOs.Common;

namespace HRManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResultDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<ResultDto<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    }
}
