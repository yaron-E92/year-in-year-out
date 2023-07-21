using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;
internal class DbEntitiesTestCases
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

    internal static readonly IReadOnlyList<MottoEntity> Mottos = new List<MottoEntity>()
    {
        new()
        {
            ID = 1,
            Content  = "I am a motto",
        },
        new()
        {
            ID = 2,
            Content = "Inspirational quote",
        },
    };
}
