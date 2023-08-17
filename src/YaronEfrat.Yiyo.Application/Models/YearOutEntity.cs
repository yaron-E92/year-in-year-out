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

    public IList<FeelingEntity> Feelings { get; set; }

    public MottoEntity Motto { get; set; }

    public IList<PersonalEventEntity> PersonalEvents { get; set; }

    public override string ToString()
    {
        StringBuilder builder = new($"YearOutEntity(ID={ID},Motto={Motto.Content} with:\n");
        builder.AppendLine($"with {Feelings?.Count ?? 0} feelings");
        builder.AppendLine($"and with {PersonalEvents?.Count ?? 0} personal events");
        return builder.ToString();
    }
}
