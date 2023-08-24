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

    public string? Title { get; set; }

    public override string ToString()
    {
        return $"PersonalEventEntity(ID={ID},Title={Title})";
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

        PersonalEventEntity other = (PersonalEventEntity) obj;

        return ID == other.ID && Title!.Equals(other.Title);
    }

    public override int GetHashCode()
    {
        return ID;
    }
}
