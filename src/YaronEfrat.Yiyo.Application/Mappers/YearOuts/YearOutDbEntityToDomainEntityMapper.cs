using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.YearOuts;

public class YearOutDbEntityToDomainEntityMapper : IDbEntityToDomainEntityMapper<YearOutEntity, YearOut>
{
    private readonly IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> _feelingMapper;
    private readonly IDbEntityToDomainEntityMapper<MottoEntity, Motto> _mottoMapper;
    private readonly IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> _personalEventMapper;

    public YearOutDbEntityToDomainEntityMapper(IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> feelingMapper,
        IDbEntityToDomainEntityMapper<MottoEntity, Motto> mottoMapper,
        IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> personalEventMapper)
    {
        _feelingMapper = feelingMapper;
        _mottoMapper = mottoMapper;
        _personalEventMapper = personalEventMapper;
    }

    public YearOut Map(YearOutEntity dbEntity)
    {
        return new YearOut
        {
            Feelings = dbEntity.Feelings.Select(_feelingMapper.Map).ToList(),
            Motto = dbEntity.Motto != null ? _mottoMapper.Map(dbEntity.Motto) : null,
            PersonalEvents = dbEntity.PersonalEvents.Select(_personalEventMapper.Map).ToList(),
        };
    }
}
