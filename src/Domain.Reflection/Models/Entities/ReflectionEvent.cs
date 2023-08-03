namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public abstract class ReflectionEvent : Entity
{
    public string Title { get; set; }

    public override void Validate()
    {
        base.Validate();
        Title = Title.Trim();
    }
}
