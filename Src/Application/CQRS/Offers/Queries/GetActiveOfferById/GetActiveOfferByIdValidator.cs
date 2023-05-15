using System;
using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Offers.Queries.GetById
{
	public class GetActiveOfferByIdValidator : AbstractValidator<GetActiveOfferByIdQuery>
	{
		private IOfferChecker _offerChecker;
		public GetActiveOfferByIdValidator(IOfferChecker offerChecker)
		{
			_offerChecker = offerChecker;
			RuleFor(x => x.Id)
				.NotNull()
				.NotEmpty()
				.NotEqual(default(Guid));
			RuleFor(x => x.Id)
				.Must(x => IsOfferExists(x) == true)
				.WithMessage("Can not get offer. Offer is not exists");
            RuleFor(x => x.Id)
				.Must(x => IsOfferActive(x) == true)
				.WithMessage("Can not get offer. Offer is not active");
        }

		private bool IsOfferExists(Guid offerId)
		{
			return _offerChecker.IsOfferExists(offerId);
		}
        private bool IsOfferActive(Guid offerId)
        {
            return _offerChecker.IsOfferActive(offerId);
        }
    }
}

