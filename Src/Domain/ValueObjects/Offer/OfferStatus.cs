using Domain.Models;

namespace Domain.ValueObjects.Offer;

public class OfferStatus : ValueObject
{
    public bool IsActive { get; private set; }
    public OfferStatus(bool isActive)
    {
        IsActive = isActive;
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