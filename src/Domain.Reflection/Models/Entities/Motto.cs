namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

/// <summary>
/// A Motto or saying associated with the passing <see cref="YearIn"/>
/// or desired for the coming <see cref="YearOut"/>
/// </summary>
public class Motto: Entity
{
    public string Content { get; set; }

    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(Content))
        {
            throw new EntityException("Content is invalid (must be non empty/whitespace)", GetType());
        }
        Content = Content.Trim();
    }
}
