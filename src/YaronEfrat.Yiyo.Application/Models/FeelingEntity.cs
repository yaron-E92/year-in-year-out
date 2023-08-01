using System.ComponentModel.DataAnnotations.Schema;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

public class FeelingEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public virtual ICollection<PersonalEventEntity> PersonalEvents { get; set; }
}
