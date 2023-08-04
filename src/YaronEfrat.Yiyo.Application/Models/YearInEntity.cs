using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

public class YearInEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public ICollection<FeelingEntity> Feelings { get; set; }

    public MottoEntity Motto { get; set; }

    public ICollection<PersonalEventEntity> PersonalEvents { get; set; }

    public ICollection<WorldEventEntity> WorldEvents { get; set; }

    public override string ToString()
    {
        StringBuilder builder = new($"YearInEntity(ID={ID},Motto={Motto.Content} with:\n");
        builder.AppendLine($"with {Feelings?.Count ?? 0} feelings");
        builder.AppendLine($"with {PersonalEvents?.Count ?? 0} personal events");
        builder.AppendLine($"and with {WorldEvents?.Count ?? 0} world events)");
        return builder.ToString();
    }
}
