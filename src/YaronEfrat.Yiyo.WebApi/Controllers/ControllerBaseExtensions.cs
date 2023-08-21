using Microsoft.AspNetCore.Mvc;

namespace YaronEfrat.Yiyo.WebApi.Controllers;

public static class ControllerBaseExtensions
{
    internal static string ControllerRoute(this ControllerBase contoller) =>
        $"https://{contoller.CurrentHost()}/api/{contoller.ControllerName()}";

    private static string ControllerName(this ControllerBase contoller) =>
        contoller.HttpContext.GetRouteData().Values["controller"]?.ToString()!;

    private static string CurrentHost(this ControllerBase contoller) =>
        contoller.HttpContext.Request.Host.ToString();
}
