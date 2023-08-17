using System.ComponentModel.DataAnnotations.Schema;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Models;

/// <summary>
/// A database oriented representation of <see cref="Feeling"/>
/// </summary>
public class FeelingEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public IList<PersonalEventEntity> PersonalEvents { get; set; } = new List<PersonalEventEntity>();

    public override string ToString()
    {
        return $"FeelingEntity(ID={ID},Title={Title} with {PersonalEvents?.Count ?? 0} personal events)";
    }
}
