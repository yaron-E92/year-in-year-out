using System.ComponentModel.DataAnnotations.Schema;
using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

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
