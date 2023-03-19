namespace Domain.Common.Exceptions;

public class LoginException : Exception
{
    public LoginException(string message) : base(message)
    {
        
    }
}