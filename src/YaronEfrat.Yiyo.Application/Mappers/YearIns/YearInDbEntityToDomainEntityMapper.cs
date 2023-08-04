using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.YearIns;

public class YearInDbEntityToDomainEntityMapper : IDbEntityToDomainEntityMapper<YearInEntity, YearIn>
{
    private readonly IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> _feelingMapper;
    private readonly IDbEntityToDomainEntityMapper<MottoEntity, Motto> _mottoMapper;
    private readonly IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> _personalEventMapper;
    private readonly IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent> _worldEventMapper;

    public YearInDbEntityToDomainEntityMapper(IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> feelingMapper,
        IDbEntityToDomainEntityMapper<MottoEntity, Motto> mottoMapper,
        IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> personalEventMapper,
        IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent> worldEventMapper)
    {
        _feelingMapper = feelingMapper;
        _mottoMapper = mottoMapper;
        _personalEventMapper = personalEventMapper;
        _worldEventMapper = worldEventMapper;
    }

    public YearIn Map(YearInEntity dbEntity)
    {
        return new YearIn
        {
            Feelings = dbEntity.Feelings?.Select(_feelingMapper.Map).ToList()!,
            Motto = _mottoMapper.Map(dbEntity.Motto),
            PersonalEvents = dbEntity.PersonalEvents?.Select(_personalEventMapper.Map).ToList()!,
            WorldEvents = dbEntity.WorldEvents?.Select(_worldEventMapper.Map).ToList()!,
        };
    }
}
