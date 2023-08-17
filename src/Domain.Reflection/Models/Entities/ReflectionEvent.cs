namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public abstract class ReflectionEvent : Entity
{
    public string Title { get; set; }

    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new EntityException("Title is invalid (must be non empty/whitespace)", GetType());
        }
        Title = Title.Trim();
    }
}
