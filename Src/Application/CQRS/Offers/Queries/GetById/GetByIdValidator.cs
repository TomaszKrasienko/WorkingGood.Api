using System;
using FluentValidation;

namespace Application.CQRS.Offers.Queries.GetById
{
	public class GetByIdValidator : AbstractValidator<GetByIdQuery>
	{
		public GetByIdValidator()
		{
			RuleFor(x => x.Id)
				.NotNull()
				.NotEmpty();
		}
	}
}

