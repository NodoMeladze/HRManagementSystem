using FluentValidation;
using HRManagement.Application.DTOs.Auth;

namespace HRManagement.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("მომხმარებლის სახელი სავალდებულოა");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("პაროლი სავალდებულოა");
        }
    }
}
