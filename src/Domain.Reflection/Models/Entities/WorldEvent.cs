namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public class WorldEvent : ReflectionEvent
{
    public IList<Source> Sources { get; set; }
    public override void Validate()
    {
        base.Validate();
        throw new NotImplementedException();
    }
}
