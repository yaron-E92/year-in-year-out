using System.ComponentModel.DataAnnotations.Schema;
using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

public class MottoEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Content { get; set; }
}
