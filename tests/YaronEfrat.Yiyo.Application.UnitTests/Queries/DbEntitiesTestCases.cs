using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;
internal class DbEntitiesTestCases
{
    public const string Sad = "Sad";
    public const string Happy = "Happy";
    public const string IAmAMotto = "I am a motto";
    public const string InspirationalQuote = "Inspirational quote";
    public const string MovedToBerlin = "Moved to Berlin";
    public const string SawTheMoon = "Saw the moon";
    public const string Corona = "Corona";
    public const string War = "War";

    public static readonly Uri Source1 = new("http://source1.net");
    public static readonly Uri Source2 = new("http://source2.net");

    internal static readonly IReadOnlyList<FeelingEntity> Feelings = new List<FeelingEntity>()
    {
        new()
        {
            ID = 1,
            Title = Sad,
        },
        new()
        {
            ID = 2,
            Title = Happy,
        },
    };

    internal static readonly IReadOnlyList<MottoEntity> Mottos = new List<MottoEntity>()
    {
        new()
        {
            ID = 1,
            Content  = IAmAMotto,
        },
        new()
        {
            ID = 2,
            Content = InspirationalQuote,
        },
    };

    internal static readonly IReadOnlyList<PersonalEventEntity> PersonalEvents = new List<PersonalEventEntity>()
    {
        new()
        {
            ID = 1,
            Title = MovedToBerlin,
        },
        new()
        {
            ID = 2,
            Title = SawTheMoon,
        },
    };

    internal static readonly IReadOnlyList<SourceEntity> Sources = new List<SourceEntity>()
    {
        new()
        {
            ID = 1,
            Url  = Source1,
        },
        new()
        {
            ID = 2,
            Url  = Source2,
        },
    };

    internal static readonly IReadOnlyList<WorldEventEntity> WorldEvents = new List<WorldEventEntity>()
    {
        new()
        {
            ID = 1,
            Title = Corona,
        },
        new()
        {
            ID = 2,
            Title = War,
        },
    };
}
