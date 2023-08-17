using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.Mottos;

public class MottoDbEntityToDomainEntityMapper : IDbEntityToDomainEntityMapper<MottoEntity, Motto>
{
    public Motto Map(MottoEntity dbEntity)
    {
        return new Motto { Content = dbEntity.Content};
    }
}
