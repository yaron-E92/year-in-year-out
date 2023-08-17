namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

/// <summary>
/// A feeling/emotion associated with the passing <see cref="YearIn"/>,
/// with a specific <see cref="PersonalEvent"/> in the passing <see cref="YearIn"/>
/// or a desired feeling/emotion for the coming <see cref="YearOut"/>
/// </summary>
public class Feeling : Entity
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// A list of all personal events referenced by this <see cref="Feeling"/>
    /// </summary>
    public IList<PersonalEvent> PersonalEvents { get; set; } = new List<PersonalEvent>();

    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new EntityException("Title is invalid (must be non empty/whitespace)", GetType());
        }
        Title = Title.Trim();

        if (string.IsNullOrWhiteSpace(Description))
        {
            throw new EntityException("Description is invalid (must be non empty/whitespace)", GetType());
        }
        Description = Description.Trim();

        PersonalEvents ??= new List<PersonalEvent>();

        foreach (PersonalEvent personalEvent in PersonalEvents)
        {
            personalEvent.Validate();
        }
    }
}
