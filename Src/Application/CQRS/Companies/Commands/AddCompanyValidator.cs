using System;
using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Companies.Commands
{
	public class AddCompanyValidator : AbstractValidator<AddCompanyCommand>
	{
		public AddCompanyValidator(ICompanyChecker companyChecker)
		{
			RuleFor(x => x.CompanyDto!.Name)
				.NotNull()
				.NotEmpty();
			RuleFor(x => x.CompanyDto!.Name)
				.Must(x => companyChecker.IsCompanyExists(x))
				.WithMessage("Company with this name already exists");
		}
	}
}

