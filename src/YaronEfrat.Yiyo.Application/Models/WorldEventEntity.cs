using System.ComponentModel.DataAnnotations.Schema;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Models;

/// <summary>
/// A database oriented representation of <see cref="WorldEvent"/>
/// </summary>
public class WorldEventEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public IList<SourceEntity> Sources { get; set; } = new List<SourceEntity>();

    public string? Title { get; set; }

    public override string ToString()
    {
        return $"WorldEventEntity(ID={ID},Title={Title} with {Sources?.Count ?? 0} sources)";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        WorldEventEntity other = (WorldEventEntity) obj;

        return ID == other.ID && Title!.Equals(other.Title)
            && Sources.SequenceEqual(other.Sources);
    }

    public override int GetHashCode()
    {
        return ID;
    }
}
