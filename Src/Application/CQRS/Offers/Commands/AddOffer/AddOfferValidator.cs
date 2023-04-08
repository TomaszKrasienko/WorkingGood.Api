using FluentValidation;

namespace Application.CQRS.Offers.Commands;

public class AddOfferValidator : AbstractValidator<AddOfferCommand>
{
    public AddOfferValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.OfferDto)
            .NotNull();
        When(x => x.OfferDto != null, () =>
        {
            RuleFor(x => x.OfferDto.Title)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.OfferDto.PositionType)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.OfferDto.SalaryRangeMin)
                .NotNull()
                .GreaterThan(500);
            RuleFor(x => x.OfferDto.SalaryRangeMax)
                .NotNull();
            RuleFor(x => x.OfferDto.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(30);
        });
    }
}