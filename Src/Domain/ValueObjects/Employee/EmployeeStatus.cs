namespace Domain.ValueObjects;

public class EmployeeStatus : ValueObject
{
    public bool IsActive { get; private set; }

    public EmployeeStatus()
    {
        IsActive = false;
    }
    internal void ChangeStatus()
    {
        IsActive = !IsActive;
    }
    public override IEnumerable<object> GetAtomicValue()
    {
        yield return IsActive;
    }
}