using MediatR;

using Microsoft.Extensions.Logging;

using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Validators;

public class YearInCommandValidator : CommandValidator<YearInEntity>
{
    public YearInCommandValidator(IMediator mediator, ILogger<YearInCommandValidator> logger) : base(mediator, logger)
    { }

    public override async Task<bool> IsValidAddCommand(IRequest<YearInEntity> request)
    {
        if (!await base.IsValidAddCommand(request).ConfigureAwait(false))
        {
            return false;
        }

        YearInEntity yearInEntity = request.GetRequestContent()!;
        IList<Task<bool>> consistencyChecks = new List<Task<bool>>
        {
            AreFeelingsConsistent(yearInEntity.Feelings),
            IsMottoConsistent(yearInEntity.Motto!),
            ArePersonalEventsConsistent(yearInEntity.PersonalEvents),
            AreWorldEventsConsistent(yearInEntity.WorldEvents),
        };
        return (await Task.WhenAll(consistencyChecks)).All(cc => cc);
    }
}
