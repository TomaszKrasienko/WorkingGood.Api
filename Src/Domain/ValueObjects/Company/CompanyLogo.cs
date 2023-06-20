namespace Domain.ValueObjects.Company;

public sealed class CompanyLogo : ValueObject
{
    public string? Logo { get; private set; }

    public CompanyLogo(string? logo)
    {
        Logo = logo;
    }
    public override IEnumerable<object> GetAtomicValue()
    {
        yield return Logo;
    }
}