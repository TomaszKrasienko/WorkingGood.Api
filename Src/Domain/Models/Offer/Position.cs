namespace Domain.Models.Offer;

public class Position : Entity<Guid>
{
    public string Type { get; private set; }
    internal Position(string type) : base(Guid.NewGuid())
    {
        Type = type;
    }
}