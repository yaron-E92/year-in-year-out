using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Models;

/// <summary>
/// A database oriented representation of <see cref="YearOut"/>
/// </summary>
public class YearOutEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public IList<FeelingEntity> Feelings { get; set; } = new List<FeelingEntity>();

    public MottoEntity? Motto { get; set; }

    public IList<PersonalEventEntity> PersonalEvents { get; set; } = new List<PersonalEventEntity>();

    public override string ToString()
    {
        StringBuilder builder = new($"YearOutEntity(ID={ID},Motto={Motto?.Content} with:\n");
        builder.AppendLine($"with {Feelings.Count} feelings");
        builder.AppendLine($"and with {PersonalEvents.Count} personal events");
        return builder.ToString();
    }
}
