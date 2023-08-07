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

    public ICollection<SourceEntity> Sources { get; set; }

    public string Title { get; set; }

    public override string ToString()
    {
        return $"WorldEventEntity(ID={ID},Title={Title} with {Sources?.Count ?? 0} sources)";
    }
}
