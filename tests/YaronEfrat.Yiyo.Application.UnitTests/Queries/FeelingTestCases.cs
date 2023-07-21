using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;
internal class FeelingTestCases
{
    internal static readonly IReadOnlyList<FeelingEntity> Feelings = new List<FeelingEntity>()
    {
        new()
        {
            ID = 1,
            Title = "Sad",
        },
        new()
        {
            ID = 2,
            Title = "Happy",
        },
    };
}
