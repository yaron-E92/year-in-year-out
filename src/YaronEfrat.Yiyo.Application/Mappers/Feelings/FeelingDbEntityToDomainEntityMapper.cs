using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Mappers.Feelings;

public class FeelingDbEntityToDomainEntityMapper : IDbEntityToDomainEntityMapper<FeelingEntity, Feeling>
{
    private readonly IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> _personalEventMapper;

    public FeelingDbEntityToDomainEntityMapper(IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> personalEventMapper)
    {
        _personalEventMapper = personalEventMapper;
    }

    public Feeling Map(FeelingEntity dbEntity)
    {
        return new Feeling
        {
            Title = dbEntity.Title,
            Description = dbEntity.Description,
            PersonalEvents = dbEntity.PersonalEvents?.Select(_personalEventMapper.Map).ToList()!,
        };
    }
}
