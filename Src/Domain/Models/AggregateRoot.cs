namespace Domain.Models;

public class AggregateRoot<T> : Entity<T>
{
    protected AggregateRoot(T id) : base(id)
    {
    }
}