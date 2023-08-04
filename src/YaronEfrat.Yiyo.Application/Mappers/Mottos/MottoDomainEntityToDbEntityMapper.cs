using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.Mottos;

public class MottoDomainEntityToDbEntityMapper : IDomainEntityToDbEntityMapper<Motto, MottoEntity>
{
    public void Map(Motto domainEntity, MottoEntity existingDbEntity)
    {
        existingDbEntity.Content = domainEntity.Content;
    }
}
