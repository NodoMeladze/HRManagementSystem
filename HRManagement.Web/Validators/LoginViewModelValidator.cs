using FluentValidation;
using HRManagement.Web.Models.API;

namespace HRManagement.Web.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("მომხმარებლის სახელი სავალდებულოა");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("პაროლი სავალდებულოა");
        }
    }
}