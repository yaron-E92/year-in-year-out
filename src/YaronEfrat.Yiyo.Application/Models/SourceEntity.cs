using System.ComponentModel.DataAnnotations.Schema;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Models;

public class SourceEntity : IDbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string? Url { get; set; }

    public override string ToString()
    {
        return $"SourceEntity(ID={ID},Url={Url})";
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

        SourceEntity other = (SourceEntity) obj;

        return ID == other.ID && Url!.Equals(other.Url);
    }

    public override int GetHashCode()
    {
        return ID;
    }
}
