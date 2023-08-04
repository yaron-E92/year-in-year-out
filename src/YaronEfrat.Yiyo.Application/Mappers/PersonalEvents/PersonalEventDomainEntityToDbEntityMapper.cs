using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;

public class PersonalEventDomainEntityToDbEntityMapper : IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity>
{
    public void Map(PersonalEvent domainEntity, PersonalEventEntity existingDbEntity)
    {
        throw new NotImplementedException();
    }
}
