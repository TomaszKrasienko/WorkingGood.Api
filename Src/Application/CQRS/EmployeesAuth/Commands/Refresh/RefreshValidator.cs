using Application.DTOs.EmployeesAuth;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshValidator : AbstractValidator<RefreshCommand>
{
    public RefreshValidator()
    {
        RuleFor(x => x.RefreshDto)
            .NotNull();
        When(x => x.RefreshDto != null, ()=> {
            RuleFor(x => x.RefreshDto.RefreshToken)
                .NotNull()
                .NotEmpty();
        });
    }
}