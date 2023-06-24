using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Services;
using Infrastructure.Tests.Helpers;

namespace Infrastructure.Tests.Services.Tests;

public class TokenProviderTests
{
    private readonly JwtConfig _jwtConfig;
    public TokenProviderTests()
    {
        _jwtConfig = new();
    }
    [Fact]
    public void Provide_ForEmailAddressRolesAndUserId_ShouldReturnLoginToken()
    {
        //Arrange
        string emailAddress = "email@address.test.pl";
        List<string> roles = new()
        {
            "test"
        };
        Guid employeeId = Guid.NewGuid();
        Guid companyId = Guid.NewGuid();
        _jwtConfig.TokenKey = "my top secret key";
        _jwtConfig.Audience = "test_audience";
        _jwtConfig.Issuer = "test_issuer";
        TokenProvider tokenProvider = new TokenProvider(_jwtConfig);
        //Act
        var result = tokenProvider.Provide(
            emailAddress,
            roles,
            employeeId,
            companyId
            );
        //Assert
        result.Should().BeOfType<LoginToken>();
        result.Token.Should().NotBeNullOrEmpty();
        TokenReader.GetEmployeeIdFromToken(result.Token).Should().Be(employeeId);
        TokenReader.GetRolesFromToken(result.Token).Should().BeEquivalentTo(roles);
        TokenReader.GetEmployeeEmailFromToken(result.Token).Should().Be(emailAddress);
    }
}