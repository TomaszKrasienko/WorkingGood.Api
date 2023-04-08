namespace Domain.Models.Offer;

public class Position
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    internal Position(string type)
    {
        Type = type;
    }
}