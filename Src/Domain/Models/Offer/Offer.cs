using Domain.Interfaces;
using Domain.ValueObjects.Offer;

namespace Domain.Models.Offer;

public class Offer : AggregateRoot<Guid>
{
    public OfferContent Content { get; set; }
    public Position Position { get; private set; }
    public SalaryRanges? SalaryRanges { get; private set; }
    public Guid AuthorId { get; private set; }
    public OfferStatus OfferStatus { get; private set; }
    public Offer() : base(Guid.NewGuid())
    {
        
    }
    public Offer(
        string title, 
        string positionType, 
        double salaryRangesValueMin, 
        double salaryRangesValueMax,
        string description,
        Guid authorId,
        bool isActive
        ) : base(Guid.NewGuid())
    {
        Content = new OfferContent(title, description);
        Position = new(positionType);
        SalaryRanges = new SalaryRanges(salaryRangesValueMin, salaryRangesValueMax);
        AuthorId = authorId;
        OfferStatus = new OfferStatus(isActive);
    }

    public void EditOffer(
        string title,
        double salaryRangesValueMin,
        double salaryRangesValueMax,
        string description,
        bool isActive)
    {
        Content = new (title, description);
        SalaryRanges = new(salaryRangesValueMin, salaryRangesValueMax);
        OfferStatus = new OfferStatus(isActive);
    }
    public void ChangeStatus()
    {
        OfferStatus.ChangeStatus();
    }
}