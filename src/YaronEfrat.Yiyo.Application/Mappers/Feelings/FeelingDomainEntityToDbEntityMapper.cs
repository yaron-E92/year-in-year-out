using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.Feelings;

public class FeelingDomainEntityToDbEntityMapper : IDomainEntityToDbEntityMapper<Feeling, FeelingEntity>
{
    private readonly IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> _personalEventMapper;

    public FeelingDomainEntityToDbEntityMapper(IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> personalEventMapper)
    {
        _personalEventMapper = personalEventMapper;
    }

    public void Map(Feeling domainEntity, FeelingEntity existingDbEntity)
    {
        existingDbEntity.Title = domainEntity.Title;
        existingDbEntity.Description = domainEntity.Description;
        _personalEventMapper.MapMany(domainEntity.PersonalEvents, existingDbEntity.PersonalEvents);
    }
}
