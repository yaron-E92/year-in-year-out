using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.YearOuts;

public class YearOutDomainEntityToDbEntityMapper : IDomainEntityToDbEntityMapper<YearOut, YearOutEntity>
{
    private readonly IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> _feelingMapper;
    private readonly IDomainEntityToDbEntityMapper<Motto, MottoEntity> _mottoMapper;
    private readonly IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> _personalEventMapper;

    public YearOutDomainEntityToDbEntityMapper(IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> feelingMapper,
        IDomainEntityToDbEntityMapper<Motto, MottoEntity> mottoMapper,
        IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> personalEventMapper)
    {
        _feelingMapper = feelingMapper;
        _mottoMapper = mottoMapper;
        _personalEventMapper = personalEventMapper;
    }

    public void Map(YearOut domainEntity, YearOutEntity existingDbEntity)
    {
        _feelingMapper.MapMany(domainEntity.Feelings, existingDbEntity.Feelings);
        if (existingDbEntity.Motto != null)
        {
            _mottoMapper.Map(domainEntity.Motto!, existingDbEntity.Motto);
        }
        _personalEventMapper.MapMany(domainEntity.PersonalEvents, existingDbEntity.PersonalEvents);
    }
}
