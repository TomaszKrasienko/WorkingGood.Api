using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.DTOs.Companies.Validators;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}