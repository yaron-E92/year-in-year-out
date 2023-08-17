using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Models;

/// <summary>
/// A database oriented representation of <see cref="YearIn"/>
/// </summary>
public class YearInEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public IList<FeelingEntity> Feelings { get; set; } = new List<FeelingEntity>();

    public MottoEntity? Motto { get; set; }

    public IList<PersonalEventEntity> PersonalEvents { get; set; } = new List<PersonalEventEntity>();

    public IList<WorldEventEntity> WorldEvents { get; set; } = new List<WorldEventEntity>();

    public override string ToString()
    {
        StringBuilder builder = new($"YearInEntity(ID={ID},Motto={Motto?.Content} with:\n");
        builder.AppendLine($"with {Feelings.Count} feelings");
        builder.AppendLine($"with {PersonalEvents.Count} personal events");
        builder.AppendLine($"and with {WorldEvents.Count} world events)");
        return builder.ToString();
    }
}
