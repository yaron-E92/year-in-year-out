using System.Reflection;

using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application;
internal static class RequestExtensions
{
    public static T? GetRequestContent<T>(this IRequest<T> request) where T : class, IDbEntity
    {
        T? content = request.GetType().GetRuntimeProperties().Where(
            info => info.PropertyType == typeof(T)).ToArray()[0].GetValue(request) as T;
        return content;
    }

    public static bool IsValidAddCommand<T>(this IRequest<T> request) where T : class, IDbEntity
    {
        return request != null! && request.GetRequestContent() is {ID: 0};
    }


}
