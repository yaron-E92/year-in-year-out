namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

public class Motto: Entity
{
    public string Content { get; set; }

    public override void Validate()
    {
        throw new NotImplementedException();
    }
}
