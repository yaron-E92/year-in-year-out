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
        Title = Title.Trim();
        Description = Description.Trim();
        PersonalEvents ??= new List<PersonalEvent>();

        foreach (PersonalEvent personalEvent in PersonalEvents)
        {
            personalEvent.Validate();
        }
    }
}
