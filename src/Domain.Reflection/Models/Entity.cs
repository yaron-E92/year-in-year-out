namespace YaronEfrat.Yiyo.Domain.Reflection.Models;

public abstract class Entity
{
    private int? _requestedHashCode;
    public int Id { get; protected set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity item)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (GetType() != obj.GetType())
            return false;
        return item.Id == Id;
    }

    public override int GetHashCode()
    {
        _requestedHashCode ??= Id.GetHashCode() ^ 31;
        // XOR for random distribution. See:
        // https://learn.microsoft.com/archive/blogs/ericlippert/guidelines-and-rules-for-gethashcode
        return _requestedHashCode.Value;
    }

    public virtual void Validate()
    {
        if (Id < 0)
        {
            throw new EntityException("Id must be non negative");
        }
    }
}
