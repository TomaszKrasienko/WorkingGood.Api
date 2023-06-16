using Application.EmployeesAuth.Commands;
using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.RegisterEmployee
{
	public class RegisterEmployeeValidator : AbstractValidator<RegisterEmployeeCommand>
	{
        private readonly IEmployeeChecker _employeeChecker;
        private readonly ICompanyChecker _companyChecker;
		public RegisterEmployeeValidator(IEmployeeChecker employeeChecker, ICompanyChecker companyChecker)
		{
            _employeeChecker = employeeChecker;
            _companyChecker = companyChecker;
            RuleFor(x => x.RegisterEmployeeDto.Email)
                .NotNull()
                .EmailAddress();
            When(x => !(x.RegisterEmployeeDto == null || x.RegisterEmployeeDto.Email == null), () =>
            {
                RuleFor(x => x.RegisterEmployeeDto.Email)
                    .Must(x => _employeeChecker.IsEmployeeExists(x) == false)
                    .WithMessage("Email already exists");
            });
            RuleFor(x => x.RegisterEmployeeDto.FirstName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(60);
            RuleFor(x => x.RegisterEmployeeDto.LastName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(60);
            RuleFor(x => x.RegisterEmployeeDto.Password)
                .NotNull()
                .NotEmpty();
            When(x => !(x.RegisterEmployeeDto == null || x.RegisterEmployeeDto.Password == null), () =>
            {
                RuleFor(x => x.RegisterEmployeeDto.Password)
                    .Must(x => IsPasswordValid(x) == true)
                    .WithMessage("Password must contains uppercase, lowercase and number");
            });
            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty();
            When(x => x.CompanyId != null, () =>
            {
                RuleFor(x => x.CompanyId)
                    .NotEmpty()
                    .NotNull()
                    .Must(x => _companyChecker.IsCompanyExists((Guid)x))
                    .WithMessage("Company is not exists");
            });
        }
        private bool IsPasswordValid(string password)
        {
            if (!(password.Any(char.IsUpper)) || !(password.Any(char.IsLower)) || !(password.Any(char.IsNumber)))
                return false;
            return true;
        }
    }
}

