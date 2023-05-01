using System.Security.AccessControl;
using Domain.Common.Exceptions;

namespace Domain.ValueObjects.Offer;

public class OfferContent : ValueObject
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public OfferContent(string title, string description)
    {
        if (string.IsNullOrEmpty(title))
            throw new DomainLogicException("Title can not be null or empty");
        if(string.IsNullOrEmpty(description))
            throw new DomainLogicException("Description can not be null or empty");
        Title = title;
        Description = description;
    }
    public override IEnumerable<object> GetAtomicValue()
    {
        yield return Title;
        yield return Description;
    }
}