using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.YearIns;

public class YearInDomainEntityToDbEntityMapper : IDomainEntityToDbEntityMapper<YearIn, YearInEntity>
{
    private readonly IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> _feelingMapper;
    private readonly IDomainEntityToDbEntityMapper<Motto, MottoEntity> _mottoMapper;
    private readonly IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> _personalEventMapper;
    private readonly IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity> _worldEventMapper;

    public YearInDomainEntityToDbEntityMapper(IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> feelingMapper,
        IDomainEntityToDbEntityMapper<Motto, MottoEntity> mottoMapper,
        IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> personalEventMapper,
        IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity> worldEventMapper)
    {
        _feelingMapper = feelingMapper;
        _mottoMapper = mottoMapper;
        _personalEventMapper = personalEventMapper;
        _worldEventMapper = worldEventMapper;
    }

    public void Map(YearIn domainEntity, YearInEntity existingDbEntity)
    {
        _feelingMapper.MapMany(domainEntity.Feelings, existingDbEntity.Feelings);
        _mottoMapper.Map(domainEntity.Motto, existingDbEntity.Motto);
        _personalEventMapper.MapMany(domainEntity.PersonalEvents, existingDbEntity.PersonalEvents);
        _worldEventMapper.MapMany(domainEntity.WorldEvents, existingDbEntity.WorldEvents);
    }
}
