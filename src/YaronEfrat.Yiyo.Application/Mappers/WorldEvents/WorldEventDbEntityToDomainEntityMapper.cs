using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.WorldEvents;

public class WorldEventDbEntityToDomainEntityMapper : IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent>
{
    public WorldEvent Map(WorldEventEntity dbEntity)
    {
        return new WorldEvent
        {
            Title = dbEntity.Title,
            Sources = MapSources(dbEntity.Sources),
        };
    }

    private static IList<Source> MapSources(IEnumerable<SourceEntity> dbSources)
    {
        return dbSources.Select(dbSource => new Source(dbSource.Url)).ToList();
    }
}
