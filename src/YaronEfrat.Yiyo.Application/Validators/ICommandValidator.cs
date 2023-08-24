using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.Validators;

public interface ICommandValidator<in T> where T : class, IDbEntity
{
    public Task<bool> IsValidAddCommand(IRequest<T> request);
}
