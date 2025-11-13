using FluentValidation;
using HRManagement.Web.Models.API;

namespace HRManagement.Web.Validators
{
    public class PositionViewModelValidator : AbstractValidator<PositionViewModel>
    {
        public PositionViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("თანამდებობის დასახელება სავალდებულოა")
                .MaximumLength(200).WithMessage("თანამდებობის დასახელება არ უნდა აღემატებოდეს 200 სიმბოლოს");
        }
    }
}