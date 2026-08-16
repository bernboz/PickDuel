namespace PickDuel.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
        {
            return false;
        }
        
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (ReferenceEquals(right, left))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }
        
        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}