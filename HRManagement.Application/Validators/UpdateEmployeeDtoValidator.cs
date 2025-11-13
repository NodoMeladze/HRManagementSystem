using FluentValidation;
using HRManagement.Application.DTOs.Employee;
using HRManagement.Application.Validators.Extensions;
using HRManagement.Domain.Enums;

namespace HRManagement.Application.Validators
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("თანამშრომლის ID სავალდებულოა");

            RuleFor(x => x.PersonalNumber).PersonalNumber();
            RuleFor(x => x.FirstName).FirstName();
            RuleFor(x => x.LastName).LastName();
            RuleFor(x => x.DateOfBirth).BirthDate();
            RuleFor(x => x.Gender).RequiredEnum("სქესი");
            RuleFor(x => x.Status).RequiredEnum("სტატუსი");

            RuleFor(x => x.Email)
                .OptionalEmail()
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.PositionId)
                .NotEmpty().WithMessage("თანამდებობა სავალდებულოა");

            RuleFor(x => x.ReleaseDate)
                .Must((dto, releaseDate) => BeValidReleaseDate(dto.Status, releaseDate))
                .WithMessage("გათავისუფლების თარიღი სავალდებულოა როდესაც სტატუსია 'გათავისუფლებული'");
        }

        private static bool BeValidReleaseDate(EmployeeStatus status, DateOnly? releaseDate)
        {
            if (status == EmployeeStatus.Released)
            {
                return releaseDate.HasValue;
            }
            return true;
        }
    }
}
