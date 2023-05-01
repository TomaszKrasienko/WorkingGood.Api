using Domain.ValueObjects;

namespace Domain.Services;

public interface ITokenProvider
{
    LoginToken Provide(string emailAddress, List<string> roles, string userId);
}