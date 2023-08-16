namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public abstract class Part : Entity
{
    public IList<PersonalEvent> PersonalEvents { get; set; }

    public IList<Feeling> Feelings { get; set; }

    public Motto Motto { get; set; }

    public override void Validate()
    {
        base.Validate();

        Motto.Validate();

        PersonalEvents ??= new List<PersonalEvent>();
        foreach (PersonalEvent personalEvent in PersonalEvents)
        {
            personalEvent.Validate();
        }

        Feelings ??= new List<Feeling>();
        foreach (Feeling feeling in Feelings)
        {
            feeling.Validate();
        }
    }
}
