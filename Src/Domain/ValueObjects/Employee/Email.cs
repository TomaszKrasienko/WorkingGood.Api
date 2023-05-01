using Domain.Common.Exceptions;

namespace Domain.ValueObjects;

public class Email : ValueObject
{
    public string EmailAddress { get; private init; }
    internal Email(string emailAddress)
    {
        if (string.IsNullOrEmpty(emailAddress))
            throw new DomainLogicException("Email can not be null or empty");
        if(emailAddress.Length >= 60)
            throw new DomainLogicException("Email length can not be longer than 60 signs");
        EmailAddress = emailAddress;
    }
    public override IEnumerable<object> GetAtomicValue()
    {
        yield return EmailAddress;
    }
}