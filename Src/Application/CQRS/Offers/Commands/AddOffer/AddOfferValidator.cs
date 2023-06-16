using FluentValidation;

namespace Application.CQRS.Offers.Commands;

public class AddOfferValidator : AbstractValidator<AddOfferCommand>
{
    public AddOfferValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Employee is not valid logged in");
        RuleFor(x => x.OfferDto)
            .NotNull()
            .WithMessage("Form can must be filled");
        When(x => x.OfferDto != null, () =>
        {
            RuleFor(x => x.OfferDto.Title)
                .NotNull()
                .WithMessage("Title must be filled")
                .NotEmpty()
                .WithMessage("Title not be empty");
            RuleFor(x => x.OfferDto.PositionType)
                .NotNull()
                .WithMessage("Position must be filled")
                .NotEmpty()
                .WithMessage("Position can not be empty");
            RuleFor(x => x.OfferDto.SalaryRangeMin)
                .NotNull()
                .WithMessage("Minimal salary must be filled")
                .GreaterThan(500)
                .WithMessage("Minimal salary must be grater than 500");
            RuleFor(x => x.OfferDto.SalaryRangeMax)
                .NotNull()
                .WithMessage("Minimal salary must be filled");
            RuleFor(x => x.OfferDto.Description)
                .NotNull()
                .WithMessage("Description must be filled")
                .NotEmpty()
                .WithMessage("Description can not be empty")
                .MinimumLength(30)
                .WithMessage("Description must have at least 30 characters");
            RuleFor(x => x.OfferDto.IsActive)
                .NotNull()
                .NotEmpty();
        });
    }
}