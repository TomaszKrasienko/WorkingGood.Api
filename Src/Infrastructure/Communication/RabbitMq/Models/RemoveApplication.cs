namespace Infrastructure.Communication.Models;

public record RemoveApplication
{
    public Guid OfferId { get; init; }
}