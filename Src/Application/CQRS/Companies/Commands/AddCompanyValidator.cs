using System;
using FluentValidation;

namespace Application.CQRS.Companies.Commands
{
	public class AddCompanyValidator : AbstractValidator<AddCompanyCommand>
	{
		public AddCompanyValidator()
		{
			RuleFor(x => x.CompanyDto!.Name)
				.NotNull()
				.NotEmpty();
		}
	}
}

