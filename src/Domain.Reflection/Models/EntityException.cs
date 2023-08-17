namespace YaronEfrat.Yiyo.Domain.Reflection.Models;

public class EntityException : Exception
{
    public EntityException(string message) : base(message)
    {
    }

    public EntityException(string message, Type entityType) : base($"[{entityType.Name}]: {message}")
    {
    }
}
