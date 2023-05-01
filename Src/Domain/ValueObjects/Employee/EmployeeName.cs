using Domain.Common.Exceptions;

namespace Domain.ValueObjects;

public class EmployeeName : ValueObject
{
    public string FirstName { get; private init; }
    public string LastName { get; private init; }
    internal EmployeeName(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new DomainLogicException("First name can not be null or empty");
        if(firstName.Length >= 60)
            throw new DomainLogicException("First name length can not be longer than 60 signs");
        if (string.IsNullOrEmpty(lastName))
            throw new DomainLogicException("Last name can not be null or empty");
        if(lastName.Length >= 60)
            throw new DomainLogicException("Last name length can not be longer than 60 signs");
        FirstName = firstName;
        LastName = lastName;
    }
    public override IEnumerable<object> GetAtomicValue()
    {
        yield return FirstName;
        yield return LastName;
    }
}