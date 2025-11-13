using FluentValidation;
using HRManagement.Application.DTOs.Auth;
using HRManagement.Application.Validators.Extensions;

namespace HRManagement.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.PersonalNumber).PersonalNumber();
            RuleFor(x => x.FirstName).FirstName();
            RuleFor(x => x.LastName).LastName();
            RuleFor(x => x.Email).RequiredEmail();
            RuleFor(x => x.DateOfBirth).BirthDate();
            RuleFor(x => x.Gender).RequiredEnum("სქესი");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("პაროლი სავალდებულოა")
                .MinimumLength(6).WithMessage("პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("პაროლის დადასტურება სავალდებულოა")
                .Equal(x => x.Password).WithMessage("პაროლები არ ემთხვევა");
        }
    }
}
