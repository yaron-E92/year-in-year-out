using System.Collections.Immutable;

using MediatR;

using Microsoft.Extensions.Logging;

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
    private readonly ILogger<CommandValidator<T>> _logger;

    public CommandValidator(IMediator mediator, ILogger<CommandValidator<T>> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public virtual Task<bool> IsValidAddCommand(IRequest<T> request)
    {
        bool isRequestNonNull = request != null!;
        bool isIdNonZero = request?.GetRequestContent() is { ID: 0 };
        if (!isRequestNonNull)
        {
            _logger.LogError("Request is null");
        }
        else if (!isIdNonZero)
        {
            _logger.LogError("Entity's id is non zero in an add command");
        }
        return Task.FromResult(isRequestNonNull && isIdNonZero);
    }

    protected internal async Task<bool> AreFeelingsConsistent(IList<FeelingEntity> entityFeelings)
    {
        GetFeelingListQuery query = new()
        {
            Ids = entityFeelings.Select(f => f.ID).ToImmutableHashSet(),
        };
        FeelingEntity[] feelingEntities = await _mediator.Send(query);
        bool areFeelingsConsistent = feelingEntities.SequenceEqual(entityFeelings);
        LogIfInconsistent(areFeelingsConsistent, nameof(FeelingEntity));
        return areFeelingsConsistent;
    }

    protected internal async Task<bool> ArePersonalEventsConsistent(IList<PersonalEventEntity> entityPersonalEvents)
    {
        GetPersonalEventListQuery query = new()
        {
            Ids = entityPersonalEvents.Select(pe => pe.ID).ToImmutableHashSet(),
        };
        PersonalEventEntity[] personalEventEntities = await _mediator.Send(query);
        bool arePersonalEventsConsistent = personalEventEntities.SequenceEqual(entityPersonalEvents);
        LogIfInconsistent(arePersonalEventsConsistent, nameof(PersonalEventEntity));
        return arePersonalEventsConsistent;
    }

    protected internal async Task<bool> AreWorldEventsConsistent(IList<WorldEventEntity> entityWorldEvents)
    {
        GetWorldEventListQuery query = new()
        {
            Ids = entityWorldEvents.Select(pe => pe.ID).ToImmutableHashSet(),
        };
        WorldEventEntity[] worldEventEntities = await _mediator.Send(query);
        bool areWorldEventsConsistent = worldEventEntities.SequenceEqual(entityWorldEvents);
        LogIfInconsistent(areWorldEventsConsistent, nameof(WorldEventEntity));
        return areWorldEventsConsistent;
    }

    protected internal async Task<bool> IsMottoConsistent(MottoEntity motto)
    {
        if (motto == null!)
        {
            return true;
        }
        GetMottoQuery query = new() {Id = motto.ID};
        MottoEntity dbMottoEntity = await _mediator.Send(query);
        bool isMottoConsistent = dbMottoEntity != null!
                                 && motto.ID == dbMottoEntity.ID
                                 && motto.Content!.Equals(dbMottoEntity.Content);
        LogIfInconsistent(isMottoConsistent, nameof(MottoEntity));
        return isMottoConsistent;
    }

    private void LogIfInconsistent(bool areMembersIncosistent, string memberClassName)
    {
        if (!areMembersIncosistent)
        {
            _logger.LogError($"Request entity contained at least one inconsistent {memberClassName}");
        }
    }
}
