using FluentValidation;
using HRManagement.Application.DTOs.Position;

namespace HRManagement.Application.Validators
{
    public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
    {
        public CreatePositionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("თანამდებობის დასახელება სავალდებულოა")
                .MaximumLength(200).WithMessage("თანამდებობის დასახელება არ უნდა აღემატებოდეს 200 სიმბოლოს");
        }
    }
}
