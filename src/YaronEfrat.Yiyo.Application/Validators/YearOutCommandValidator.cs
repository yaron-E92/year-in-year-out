using MediatR;

using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Validators;

public class YearOutCommandValidator : CommandValidator<YearOutEntity>
{
    public YearOutCommandValidator(IMediator mediator) : base(mediator)
    { }

    public override async Task<bool> IsValidAddCommand(IRequest<YearOutEntity> request)
    {
        if (!await base.IsValidAddCommand(request).ConfigureAwait(false))
        {
            return false;
        }

        YearOutEntity yearOutEntity = request.GetRequestContent()!;
        IList<Task<bool>> consistencyChecks = new List<Task<bool>>
        {
            AreFeelingsConsistent(yearOutEntity.Feelings),
            IsMottoConsistent(yearOutEntity.Motto!),
            ArePersonalEventsConsistent(yearOutEntity.PersonalEvents),
        };
        return (await Task.WhenAll(consistencyChecks)).All(cc => cc);
    }
}
