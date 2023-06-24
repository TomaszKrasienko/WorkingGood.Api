using Application.DTOs.Companies.Validators;
using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Companies.Commands.EditCompany;

public class EditCompanyCommandValidator : AbstractValidator<EditCompanyCommand>
{
    public EditCompanyCommandValidator(ICompanyChecker companyChecker)
    {
        RuleFor(x => x.CompanyId)
            .NotNull()
            .NotEqual(default(Guid));
        When(x => x.CompanyId is not null, () =>
        {
            RuleFor(x => x.CompanyId)
                .Must(x => companyChecker.IsCompanyExists((Guid) x));
        });
        RuleFor(x => x.CompanyDto)
            .SetValidator(new CompanyDtoValidator());
    }
}