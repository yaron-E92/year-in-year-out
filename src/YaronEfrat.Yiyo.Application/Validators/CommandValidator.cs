using System.Collections.Immutable;

using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;
using YaronEfrat.Yiyo.Application.Queries.Feelings;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;
using YaronEfrat.Yiyo.Application.Queries.WorldEvents;

namespace YaronEfrat.Yiyo.Application.Validators;

public class CommandValidator<T> : ICommandValidator<T> where T : class, IDbEntity
{
    private readonly IMediator _mediator;

    public CommandValidator(IMediator mediator)
    {
        _mediator = mediator;
    }

    public virtual Task<bool> IsValidAddCommand(IRequest<T> request)
    {
        return Task.FromResult(request != null! && request.GetRequestContent() is { ID: 0 });
    }

    protected internal async Task<bool> AreFeelingsConsistent(IList<FeelingEntity> entityFeelings)
    {
        GetFeelingListQuery query = new()
        {
            Ids = entityFeelings.Select(f => f.ID).ToImmutableHashSet(),
        };
        FeelingEntity[] feelingEntities = await _mediator.Send(query);
        return feelingEntities.SequenceEqual(entityFeelings);
    }

    protected internal async Task<bool> ArePersonalEventsConsistent(IList<PersonalEventEntity> entityPersonalEvents)
    {
        GetPersonalEventListQuery query = new()
        {
            Ids = entityPersonalEvents.Select(pe => pe.ID).ToImmutableHashSet(),
        };
        PersonalEventEntity[] personalEventEntities = await _mediator.Send(query);
        return personalEventEntities.SequenceEqual(entityPersonalEvents);
    }

    protected internal async Task<bool> AreWorldEventsConsistent(IList<WorldEventEntity> entityWorldEvents)
    {
        GetWorldEventListQuery query = new()
        {
            Ids = entityWorldEvents.Select(pe => pe.ID).ToImmutableHashSet(),
        };
        WorldEventEntity[] worldEventEntities = await _mediator.Send(query);
        return worldEventEntities.SequenceEqual(entityWorldEvents);
    }

    protected internal async Task<bool> IsMottoConsistent(MottoEntity motto)
    {
        if (motto == null!)
        {
            return true;
        }
        GetMottoQuery query = new() {Id = motto.ID};
        MottoEntity dbMottoEntity = await _mediator.Send(query);
        return dbMottoEntity != null!
            && motto.ID == dbMottoEntity.ID && motto.Content!.Equals(dbMottoEntity.Content);
    }
}
