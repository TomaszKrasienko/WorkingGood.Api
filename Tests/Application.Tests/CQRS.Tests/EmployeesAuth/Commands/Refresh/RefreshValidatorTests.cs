using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshValidatorTests
{
    private IValidator<RefreshCommand> _validator;
    public RefreshValidatorTests()
    {
        
    }
    [Fact]
    public async Task RefreshValidator_ForValidRefreshCommand_ShouldHaveNotErrors()
    {
        //Arrange
            RefreshCommand refreshCommand = new()
            {
                RefreshDto = new()
                {
                    RefreshToken = "test123test"
                }
            };
            _validator = new RefreshValidator();
        //Act
            var result = await _validator.TestValidateAsync(refreshCommand);
        //Assert
            result.ShouldNotHaveAnyValidationErrors();
    }
    private static IEnumerable<object[]> GetInvalidRefreshCommands()
    {
        List<RefreshCommand> refreshCommand = new()
        {
            new RefreshCommand()
            {
                RefreshDto = new()
            },
            new RefreshCommand(),
            new RefreshCommand()
            {
                RefreshDto = new()
                {
                    RefreshToken = ""
                }
            }
        };
        return refreshCommand.Select(x => new object[] {x});
    }
    [Theory]
    [MemberData(nameof(GetInvalidRefreshCommands))]
    public async Task RefreshValidator_ForInvalidRefreshCommand_ShouldHaveErrors( RefreshCommand refreshCommand)
    {
        //Arrange
            _validator = new RefreshValidator();
        //Act
            var result = await _validator.TestValidateAsync(refreshCommand);
        //Assert
            result.ShouldHaveAnyValidationError();
    }
}