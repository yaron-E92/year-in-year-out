namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

/// <summary>
/// A representation of the past year coming to an end
/// to help one reflect on the previous year
/// </summary>
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
