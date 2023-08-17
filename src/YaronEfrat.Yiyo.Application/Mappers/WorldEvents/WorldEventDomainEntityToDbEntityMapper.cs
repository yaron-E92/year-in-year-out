using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.WorldEvents;

public class WorldEventDomainEntityToDbEntityMapper : IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity>
{
    public void Map(WorldEvent domainEntity, WorldEventEntity existingDbEntity)
    {
        existingDbEntity.Title = domainEntity.Title;
    }
}
