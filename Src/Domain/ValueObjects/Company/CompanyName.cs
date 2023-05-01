using Domain.Common.Exceptions;

namespace Domain.ValueObjects.Company;

public sealed class CompanyName : ValueObject
{
    public string Name { get; private set; }
    public CompanyName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainLogicException("Company name can not be null or empty");
        Name = name;
    }
    public override IEnumerable<object> GetAtomicValue()
    {
        yield return Name;
    }
}