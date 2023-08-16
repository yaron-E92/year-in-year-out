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

    public IList<FeelingEntity> Feelings { get; set; }

    public MottoEntity Motto { get; set; }

    public IList<PersonalEventEntity> PersonalEvents { get; set; }

    public IList<WorldEventEntity> WorldEvents { get; set; }

    public override string ToString()
    {
        StringBuilder builder = new($"YearInEntity(ID={ID},Motto={Motto.Content} with:\n");
        builder.AppendLine($"with {Feelings?.Count ?? 0} feelings");
        builder.AppendLine($"with {PersonalEvents?.Count ?? 0} personal events");
        builder.AppendLine($"and with {WorldEvents?.Count ?? 0} world events)");
        return builder.ToString();
    }
}
