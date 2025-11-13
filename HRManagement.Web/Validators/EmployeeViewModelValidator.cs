using FluentValidation;
using HRManagement.Web.Constants;
using HRManagement.Web.Extensions;
using HRManagement.Web.Models.API;

namespace HRManagement.Web.Validators
{
    public class EmployeeViewModelValidator : AbstractValidator<EmployeeViewModel>
    {
        public EmployeeViewModelValidator()
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
                .EmailAddress().WithMessage("ელ.ფოსტის ფორმატი არასწორია")
                .MaximumLength(255).WithMessage("ელ.ფოსტა არ უნდა აღემატებოდეს 255 სიმბოლოს")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("დაბადების თარიღი სავალდებულოა")
                .Must(BeValidAge).WithMessage(ValidationMessages.AgeOutOfRange);

            RuleFor(x => x.Gender)
                .Must(g => g == 1 || g == 2)
                .WithMessage("სქესი არასწორად არის მითითებული");

            RuleFor(x => x.PositionId)
                .NotEmpty().WithMessage("თანამდებობა სავალდებულოა")
                .Must(id => id != Guid.Empty).WithMessage("თანამდებობა სავალდებულოა");


            RuleFor(x => x.Status)
                .Must(s => s >= 0 && s <= 3)
                .WithMessage("სტატუსი არასწორად არის მითითებული");

            RuleFor(x => x.ReleaseDate)
                .NotNull()
                .WithMessage(ValidationMessages.ReleaseDateRequired)
                .When(x => x.Status == 3);
        }

        private static bool BeValidAge(DateOnly dateOfBirth)
        {
            return dateOfBirth.IsValidAge();
        }
    }
}