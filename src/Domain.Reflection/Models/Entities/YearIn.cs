namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public class YearIn : Part
{
    public IList<WorldEvent> WorldEvents { get; set; }

    public override void Validate()
    {
        base.Validate();

        WorldEvents ??= new List<WorldEvent>();

        foreach (WorldEvent worldEvent in WorldEvents)
        {
            worldEvent.Validate();
        }
    }
}
