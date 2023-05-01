namespace Domain.Models;

public class Entity<T> : IEquatable<Entity<T>>
{
    public T Id { get; init; }
    protected Entity(T id)
    {
        Id = id;
    }
    public bool Equals(Entity<T>? other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj.GetType() != GetType())
            return false;
        if (obj is not Entity<T> entity)
            return false;
        return entity.Id!.Equals(Id);
    }
    public override int GetHashCode()
    {
        return Id!.GetHashCode() * 41;
    }
    public static bool operator == (Entity<T>? first, Entity<T>? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity<T>? first, Entity<T>? second)
    {
        return first is not null && second is not null && !(first.Equals(second));
    }
}