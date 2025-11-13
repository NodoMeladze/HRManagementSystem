using FluentValidation;
using HRManagement.Web.Constants;
using HRManagement.Web.Extensions;
using HRManagement.Web.Models.API;

namespace HRManagement.Web.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.PersonalNumber)
                .NotEmpty().WithMessage("პირადი ნომერი სავალდებულოა")
                .Length(11).WithMessage("პირადი ნომერი უნდა შედგებოდეს 11 სიმბოლოსგან")
                .Matches(@"^\d{11}$").WithMessage("პირადი ნომერი უნდა შეიცავდეს მხოლოდ ციფრებს");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("სახელი სავალდებულოა")
                .MaximumLength(100).WithMessage("სახელი არ უნდა აღემატებოდეს 100 სიმბოლოს");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("გვარი სავალდებულოა")
                .MaximumLength(100).WithMessage("გვარი არ უნდა აღემატებოდეს 100 სიმბოლოს");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("ელ.ფოსტა სავალდებულოა")
                .EmailAddress().WithMessage("ელ.ფოსტის ფორმატი არასწორია")
                .MaximumLength(255).WithMessage("ელ.ფოსტა არ უნდა აღემატებოდეს 255 სიმბოლოს");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("დაბადების თარიღი სავალდებულოა")
                .Must(BeValidAge).WithMessage(ValidationMessages.AgeOutOfRange);

            RuleFor(x => x.Gender)
                .Must(g => g == 1 || g == 2)
                .WithMessage("სქესი არასწორად არის მითითებული");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("პაროლი სავალდებულოა")
                .MinimumLength(6).WithMessage("პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("პაროლის დადასტურება სავალდებულოა")
                .Equal(x => x.Password).WithMessage("პაროლები არ ემთხვევა");
        }

        private static bool BeValidAge(DateOnly dateOfBirth)
        {
            return dateOfBirth.IsValidAge();
        }
    }
}