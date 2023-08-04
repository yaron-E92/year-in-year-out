using YaronEfrat.Yiyo.Domain.Reflection.Models.Exceptions;

namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public class Feeling : Entity
{
    public string Title { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// A list of all personal events referenced by this <see cref="Feeling"/>
    /// </summary>
    public IList<PersonalEvent> PersonalEvents { get; set; }

    public override void Validate()
    {
        base.Validate();

        if (string.IsNullOrWhiteSpace(Title))
        {
            throw new FeelingException("Title is invalid (must be non empty/whitespace)");
        }
        Title = Title.Trim();

        if (string.IsNullOrWhiteSpace(Description))
        {
            throw new FeelingException("Description is invalid (must be non empty/whitespace)");
        }
        Description = Description.Trim();

        PersonalEvents ??= new List<PersonalEvent>();

        foreach (PersonalEvent personalEvent in PersonalEvents)
        {
            personalEvent.Validate();
        }
    }
}
