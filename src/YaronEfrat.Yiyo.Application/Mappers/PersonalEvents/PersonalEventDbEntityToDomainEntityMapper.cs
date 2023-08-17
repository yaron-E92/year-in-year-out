using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;

public class PersonalEventDbEntityToDomainEntityMapper : IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent>
{
    public PersonalEvent Map(PersonalEventEntity dbEntity)
    {
        return new PersonalEvent {Title = dbEntity.Title};
    }
}
