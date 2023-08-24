using System.Reflection;

using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application;
internal static class RequestExtensions
{
    public static T? GetRequestContent<T>(this IRequest<T> request) where T : class, IDbEntity
    {
        return request.GetType().GetRuntimeProperties()
            .Single(info => info.PropertyType == typeof(T))
            .GetValue(request) as T;
    }
}
