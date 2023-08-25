using MediatR;

using Microsoft.Extensions.Logging;

using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Validators;

public class FeelingCommandValidator : CommandValidator<FeelingEntity>
{
    public FeelingCommandValidator(IMediator mediator, ILogger<FeelingCommandValidator> logger) : base(mediator, logger)
    { }

    public override async Task<bool> IsValidAddCommand(IRequest<FeelingEntity> request)
    {
        if (!await base.IsValidAddCommand(request).ConfigureAwait(false))
        {
            return false;
        }

        FeelingEntity feelingEntity = request.GetRequestContent()!;
        return await ArePersonalEventsConsistent(feelingEntity.PersonalEvents);
    }
}
