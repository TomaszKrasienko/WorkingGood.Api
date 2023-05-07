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
				.Must(x => IsOfferValid(x) == true)
				.WithMessage("Can not get offer. Offer is invalid");
		}

		private bool IsOfferValid(Guid offerId)
		{
			return _offerChecker.IsOfferExists(offerId) && _offerChecker.IsOfferActive(offerId);
		}
	}
}

