using System.ComponentModel.DataAnnotations.Schema;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Models;

/// <summary>
/// A database oriented representation of <see cref="PersonalEvent"/>
/// </summary>
public class PersonalEventEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Title { get; set; }

    public override string ToString()
    {
        return $"PersonalEventEntity(ID={ID},Title={Title})";
    }
}
