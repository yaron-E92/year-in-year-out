using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.UnitTests;
internal class DbEntitiesTestCases
{
    public const string Sad = "Sad";
    public const string Happy = "Happy ";
    public const string IAmAMotto = "I am a motto ";
    public const string InspirationalQuote = "Inspirational quote";
    public const string MovedToBerlin = "Moved to Berlin ";
    public const string SawTheMoon = "Saw the moon";
    public const string Corona = "Corona ";
    public const string War = "War";
    public const string Source1 = "http://source1.net";
    public const string Source2 = "http://source2.net";
    public const string NewUrl = "https://new.net";


    internal static readonly IList<FeelingEntity> Feelings = new List<FeelingEntity>
    {
        new()
        {
            ID = 1,
            Title = Sad,
            Description = Sad,
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 2,
            Title = Happy,
            Description = Happy,
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 3,
            Title = Happy,
            Description = Happy,
            PersonalEvents = new List<PersonalEventEntity> {new()
            {
                ID = 1, 
                Title = MovedToBerlin,
            }},
        },
    };

    internal static readonly IList<MottoEntity> Mottos = new List<MottoEntity>
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

    internal static readonly IList<PersonalEventEntity> PersonalEvents = new List<PersonalEventEntity>
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

    internal static readonly IList<SourceEntity> Sources = new List<SourceEntity>
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

    internal static readonly IList<WorldEventEntity> WorldEvents = new List<WorldEventEntity>
    {
        new()
        {
            ID = 1,
            Title = Corona,
            Sources = new List<SourceEntity>
            {
                Sources[0],
                new()
                {
                    ID = Sources[1].ID,
                    Url = NewUrl,
                },
            },
        },
        new()
        {
            ID = 2,
            Title = War,
            Sources = Sources,
        },
        new()
        {
            ID = 3,
            Title = Corona,
            Sources = new List<SourceEntity>
            {
                Sources[0],
                Sources[1],
                new()
                {
                    ID = 0,
                    Url = NewUrl,
                },
            },
        },
    };

    internal static readonly IList<YearInEntity> YearIns = new List<YearInEntity>
    {
        new()
        {
            ID = 1,
            Feelings = new List<FeelingEntity>(),
            Motto = Mottos[0],
            PersonalEvents = new List<PersonalEventEntity>(),
            WorldEvents = new List<WorldEventEntity>(),
        },
        new()
        {
            ID = 2,
            Feelings = Feelings,
            Motto = Mottos[1],
            PersonalEvents = PersonalEvents,
            WorldEvents = WorldEvents,
        },
    };

    internal static readonly IList<YearOutEntity> YearOuts = new List<YearOutEntity>
    {
        new()
        {
            ID = 1,
            Feelings = new List<FeelingEntity>(),
            Motto = Mottos[0],
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 2,
            Feelings = Feelings,
            Motto = Mottos[1],
            PersonalEvents = PersonalEvents,
        },
    };

    internal static readonly IList<FeelingEntity> InvalidFeelings = new List<FeelingEntity>
    {
        new()
        {
            ID = 0,
            Title = "",
            Description = Sad,
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 0,
            Title = " ",
            Description = Sad,
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 0,
            Title = null!,
            Description = Happy,
            PersonalEvents = new List<PersonalEventEntity>(),
        },new()
        {
            ID = 0,
            Title = Sad,
            Description = "",
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 0,
            Title = Sad,
            Description = " ",
            PersonalEvents = new List<PersonalEventEntity>(),
        },
        new()
        {
            ID = 0,
            Title = Happy,
            Description = null!,
            PersonalEvents = new List<PersonalEventEntity>(),
        },
    };
}
