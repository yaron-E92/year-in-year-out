namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public abstract class Part : Entity
{
    public IList<PersonalEvent> PersonalEvents { get; set; }

    public IList<Feeling> Feelings { get; set; }

    public Motto Motto { get; set; }

    public override void Validate()
    {
        base.Validate();
        throw new NotImplementedException();
    }
}
