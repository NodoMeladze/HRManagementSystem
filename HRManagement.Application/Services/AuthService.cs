using HRManagement.Application.DTOs.Auth;
using HRManagement.Application.DTOs.Common;
using HRManagement.Application.Exceptions;
using HRManagement.Application.Interfaces;
using HRManagement.Domain.Entities;
using HRManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace HRManagement.Application.Services
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        ILogger<AuthService> logger) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ILogger<AuthService> _logger = logger;

        public async Task<ResultDto<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    if (await _unitOfWork.Users.EmailExistsAsync(registerDto.Email))
                    {
                        throw new BusinessException(
                            "ასეთი მომხმარებელი უკვე არსებობს, გთხოვთ შეხვიდეთ სისტემაში");
                    }

                    if (await _unitOfWork.Users.PersonalNumberExistsAsync(registerDto.PersonalNumber))
                    {
                        throw new BusinessException(
                            "ამ პირადი ნომრით მომხმარებელი უკვე რეგისტრირებულია");
                    }

                    var user = new User
                    {
                        PersonalNumber = registerDto.PersonalNumber,
                        FirstName = registerDto.FirstName,
                        LastName = registerDto.LastName,
                        Gender = registerDto.Gender,
                        DateOfBirth = registerDto.DateOfBirth,
                        Email = registerDto.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
                    };

                    await _unitOfWork.Users.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("User registered successfully: {Email}", user.Email);

                    var accessToken = _tokenService.GenerateAccessToken(user);

                    return new AuthResponseDto
                    {
                        Token = accessToken,
                        ExpiresAt = DateTime.UtcNow.AddHours(24),
                        User = new UserDto
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email
                        }
                    };
                });

                return ResultDto<AuthResponseDto>.SuccessResult(
                    response,
                    "რეგისტრაცია წარმატებით დასრულდა");
            }
            catch (BusinessException ex)
            {
                return ResultDto<AuthResponseDto>.FailureResult(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return ResultDto<AuthResponseDto>.FailureResult(
                    "რეგისტრაციის დროს მოხდა შეცდომა",
                    [ex.Message]);
            }
        }

        public async Task<ResultDto<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Username ||
                                            u.PersonalNumber == loginDto.Username);

                if (user == null)
                {
                    return ResultDto<AuthResponseDto>.FailureResult(
                        "მომხმარებელი ან პაროლი არასწორია",
                        ["Invalid credentials"]);
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return ResultDto<AuthResponseDto>.FailureResult(
                        "მომხმარებელი ან პაროლი არასწორია",
                        ["Invalid credentials"]);
                }

                _logger.LogInformation("User logged in successfully: {Email}", user.Email);

                var accessToken = _tokenService.GenerateAccessToken(user);

                var response = new AuthResponseDto
                {
                    Token = accessToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    User = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    }
                };

                return ResultDto<AuthResponseDto>.SuccessResult(
                    response,
                    "წარმატებით შეხვედით სისტემაში");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return ResultDto<AuthResponseDto>.FailureResult(
                    "ავტორიზაციის დროს მოხდა შეცდომა",
                    [ex.Message]);
            }
        }
    }
}