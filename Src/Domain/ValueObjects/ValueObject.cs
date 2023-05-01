namespace Domain.ValueObjects;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetAtomicValue();
    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }
    public override bool Equals(object? obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }
    public override int GetHashCode()
    {
        return GetAtomicValue().Aggregate(default(int), HashCode.Combine);
    }
    private bool ValuesAreEqual(ValueObject other)
    {
        return GetAtomicValue().SequenceEqual(other.GetAtomicValue());
    }
}