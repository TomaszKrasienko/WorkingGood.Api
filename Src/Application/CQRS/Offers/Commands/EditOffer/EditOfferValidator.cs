using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Offers.Commands.EditOffer;

public class EditOfferValidator : AbstractValidator<EditOfferCommand>
{
    public EditOfferValidator(IOfferChecker offerChecker)
    {
        RuleFor(x => x.OfferId)
            .NotNull()
            .NotEmpty()
            .NotEqual(default(Guid))
            .Must(x => offerChecker.IsOfferExists(x) == true)
            .WithMessage("Offer doesn't exists");
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
            RuleFor(x => x.OfferDto.IsActive)
                .NotNull()
                .NotEmpty();
        });
    }
}