using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Validators;

internal class CommandValidator<T> : ICommandValidator<T> where T : class, IDbEntity
{
    public virtual Task<bool> IsValidAddCommand(IRequest<T> request)
    {
        return Task.FromResult(request != null! && request.GetRequestContent() is { ID: 0 });
    }
}
