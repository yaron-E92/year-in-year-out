using System.ComponentModel.DataAnnotations.Schema;
using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

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
