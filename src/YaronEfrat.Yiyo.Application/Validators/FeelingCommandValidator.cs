using System.Collections.Immutable;

using MediatR;

using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;

namespace YaronEfrat.Yiyo.Application.Validators;

public class FeelingCommandValidator : CommandValidator<FeelingEntity>
{
    private readonly IMediator _mediator;

    public FeelingCommandValidator(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<bool> IsValidAddCommand(IRequest<FeelingEntity> request)
    {
        if (!await base.IsValidAddCommand(request).ConfigureAwait(false))
        {
            return false;
        }

        FeelingEntity feelingEntity = request.GetRequestContent()!;
        GetPersonalEventListQuery query = new()
        {
            Ids = feelingEntity.PersonalEvents.Select(pe => pe.ID).ToImmutableHashSet(),
        };
        PersonalEventEntity[] personalEventEntities = await _mediator.Send(query);
        return personalEventEntities.SequenceEqual(feelingEntity.PersonalEvents);
    }
}
