using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Companies.Commands.RegisterCompany;

public class RegisterCompanyValidator : AbstractValidator<RegisterCompanyCommand>
{
    public RegisterCompanyValidator(ICompanyChecker companyChecker, IEmployeeChecker employeeChecker)
    {
        RuleFor(x => x.RegisterCompanyDto!.CompanyName)
            .NotNull()
            .NotEmpty();
        When(x => x.RegisterCompanyDto!.CompanyName != null, () => 
        {
            RuleFor(x => x.RegisterCompanyDto!.CompanyName)
                .Must(x => !companyChecker.IsCompanyExists(x))
                .WithMessage("Company with this name already exists");
        });
        RuleFor(x => x.RegisterCompanyDto!.EmployeeEmail)
            .NotNull()
            .EmailAddress();
        When(x => !(x.RegisterCompanyDto == null || x.RegisterCompanyDto.EmployeeEmail == null), () =>
        {
            RuleFor(x => x.RegisterCompanyDto!.EmployeeEmail)
                .Must(x => employeeChecker.IsEmployeeExists(x) == false)
                .WithMessage("Email already exists");
        });
        RuleFor(x => x.RegisterCompanyDto!.EmployeeFirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(60);
        RuleFor(x => x.RegisterCompanyDto!.EmployeeLastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(60);
        RuleFor(x => x.RegisterCompanyDto!.EmployeePassword)
            .NotNull()
            .NotEmpty();
        When(x => !(x.RegisterCompanyDto == null || x.RegisterCompanyDto.EmployeePassword == null), () =>
        {
            RuleFor(x => x.RegisterCompanyDto!.EmployeePassword)
                .Must(x => IsPasswordValid(x) == true)
                .WithMessage("Password must contains uppercase, lowercase and number");
        });
    }
    private bool IsPasswordValid(string password)
    {
        if (!(password.Any(char.IsUpper)) || !(password.Any(char.IsLower)) || !(password.Any(char.IsNumber)))
            return false;
        return true;
    }
}