using Domain.Interfaces;
using Domain.ValueObjects.Offer;

namespace Domain.Models.Offer;

public class Offer : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public Position Position { get; private set; }
    public SalaryRanges? SalaryRanges { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Description { get; private set; }
    public Offer()
    {
        
    }
    public Offer(string title, string positionType, string description, Guid authorId)
    {
        Title = title;
        Position = new(positionType);
        Description = description;
        AuthorId = authorId;
    }
    public Offer(
        string title, 
        string positionType, 
        double salaryRangesValueMin, 
        double salaryRangesValueMax,
        string description,
        Guid authorId)
    {
        Title = title;
        Position = new(positionType);
        SalaryRanges = new SalaryRanges(salaryRangesValueMin, salaryRangesValueMax);
        Description = description;
        AuthorId = authorId;
    }
}