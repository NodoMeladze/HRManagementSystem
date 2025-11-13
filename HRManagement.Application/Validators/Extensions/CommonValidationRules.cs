using FluentValidation;

namespace HRManagement.Application.Validators.Extensions
{
    public static class CommonValidationRules
    {
        public static IRuleBuilderOptions<T, string> PersonalNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("პირადი ნომერი სავალდებულოა")
                .Length(11).WithMessage("პირადი ნომერი უნდა შედგებოდეს 11 სიმბოლოსგან")
                .Matches(@"^\d{11}$").WithMessage("პირადი ნომერი უნდა შეიცავდეს მხოლოდ ციფრებს");
        }

        public static IRuleBuilderOptions<T, string> FirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("სახელი სავალდებულოა")
                .MaximumLength(100).WithMessage("სახელი არ უნდა აღემატებოდეს 100 სიმბოლოს");
        }

        public static IRuleBuilderOptions<T, string> LastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("გვარი სავალდებულოა")
                .MaximumLength(100).WithMessage("გვარი არ უნდა აღემატებოდეს 100 სიმბოლოს");
        }

        public static IRuleBuilderOptions<T, string> RequiredEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("ელ.ფოსტა სავალდებულოა")
                .EmailAddress().WithMessage("ელ.ფოსტის ფორმატი არასწორია")
                .MaximumLength(255).WithMessage("ელ.ფოსტა არ უნდა აღემატებოდეს 255 სიმბოლოს");
        }

        public static IRuleBuilderOptions<T, string?> OptionalEmail<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .EmailAddress().WithMessage("ელ.ფოსტის ფორმატი არასწორია")
                .MaximumLength(255).WithMessage("ელ.ფოსტა არ უნდა აღემატებოდეს 255 სიმბოლოს")
                .When(x => !string.IsNullOrEmpty(ruleBuilder.ToString()));
        }

        public static IRuleBuilderOptions<T, DateOnly> BirthDate<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("დაბადების თარიღი სავალდებულოა")
                .Must(BeValidAge).WithMessage("ასაკი უნდა იყოს 18 წლიდან 100 წლამდე");
        }

        public static IRuleBuilderOptions<T, TEnum> RequiredEnum<T, TEnum>(this IRuleBuilder<T, TEnum> ruleBuilder, string fieldName)
            where TEnum : struct, Enum
        {
            return ruleBuilder
                .IsInEnum().WithMessage($"{fieldName} არასწორად არის მითითებული");
        }

        private static bool BeValidAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age >= 18 && age <= 100;
        }
    }
}
