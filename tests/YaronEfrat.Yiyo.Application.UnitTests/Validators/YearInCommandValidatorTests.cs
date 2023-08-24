using FluentAssertions;

using MediatR;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.YearIns;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;
using YaronEfrat.Yiyo.Application.Queries.Feelings;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;
using YaronEfrat.Yiyo.Application.Queries.WorldEvents;
using YaronEfrat.Yiyo.Application.Validators;

namespace YaronEfrat.Yiyo.Application.UnitTests.Validators;
internal class YearInCommandValidatorTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private YearInCommandValidator _validator;

    private Mock<IMediator> _mediatorMock;

    private GetFeelingListQueryHandler _getFeelingListQueryHandler;
    private GetMottoQueryHandler _getMottoQueryHandler;
    private GetPersonalEventListQueryHandler _getPersonalEventListQueryHandler;
    private GetWorldEventListQueryHandler _getWorldEventListQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        _dbContextMock.Setup(mock => mock.Feelings)
            .Returns(TestFixtures.DbSetMock(DbEntitiesTestCases.Feelings).Object);
        _dbContextMock.Setup(mock => mock.Mottos)
            .Returns(TestFixtures.DbSetMock(DbEntitiesTestCases.Mottos).Object);
        _dbContextMock.Setup(mock => mock.PersonalEvents)
            .Returns(TestFixtures.DbSetMock(DbEntitiesTestCases.PersonalEvents).Object);
        _dbContextMock.Setup(mock => mock.WorldEvents)
            .Returns(TestFixtures.DbSetMock(DbEntitiesTestCases.WorldEvents).Object);

        _getFeelingListQueryHandler = new GetFeelingListQueryHandler(_dbContextMock.Object);
        _getMottoQueryHandler = new GetMottoQueryHandler(_dbContextMock.Object);
        _getPersonalEventListQueryHandler = new GetPersonalEventListQueryHandler(_dbContextMock.Object);
        _getWorldEventListQueryHandler = new GetWorldEventListQueryHandler(_dbContextMock.Object);

        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFeelingListQuery>(), default))
            .Returns(async (GetFeelingListQuery q, CancellationToken token) =>
                await _getFeelingListQueryHandler.Handle(q, token));
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMottoQuery>(), default))
            .Returns(async (GetMottoQuery q, CancellationToken token) =>
                await _getMottoQueryHandler.Handle(q, token));
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonalEventListQuery>(), default))
            .Returns(async (GetPersonalEventListQuery q, CancellationToken token) =>
                await _getPersonalEventListQueryHandler.Handle(q, token));
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetWorldEventListQuery>(), default))
            .Returns(async (GetWorldEventListQuery q, CancellationToken token) =>
                await _getWorldEventListQueryHandler.Handle(q, token));

        _validator = new YearInCommandValidator(_mediatorMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.YearIns))]
    public async Task Should_ReturnTrue_When_ValidCommand(YearInEntity yearInEntity)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0, // Indicate non existing
                Feelings = yearInEntity.Feelings,
                Motto = yearInEntity.Motto,
                PersonalEvents = yearInEntity.PersonalEvents,
                WorldEvents = yearInEntity.WorldEvents,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeTrue();
    }

    [Test]
    public async Task Should_ReturnFalse_When_RequestNull()
    {
        // Act
        bool isValid = await _validator.IsValidAddCommand(null!);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnFalse_When_NonZeroId(int id)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = id,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearInEntityContainsNonExistantFeeling(int id)
    {
        // Arrange
        IList<FeelingEntity> feelingEntities = new List<FeelingEntity>()
        {
            new()
            {
                ID = id,
                Title = "d",
                Description = "d",
                PersonalEvents = new List<PersonalEventEntity>(),
            },
            new()
            {
                ID = 2,
                Title = DbEntitiesTestCases.Happy,
                Description = DbEntitiesTestCases.Happy,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = feelingEntities,
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearInEntityContainsAdjustedFeeling()
    {
        // Arrange
        IList<FeelingEntity> feelingEntities = new List<FeelingEntity>()
        {
            new()
            {
                ID = 1,
                Title = "d",
                Description = "d",
                PersonalEvents = new List<PersonalEventEntity>(),
            },
            new()
            {
                ID = 2,
                Title = DbEntitiesTestCases.Happy,
                Description = DbEntitiesTestCases.Happy,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = feelingEntities,
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearInEntityContainsNonExistantMotto(int id)
    {
        // Arrange
        MottoEntity motto = new() {ID = id, Content = "d"};
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = motto,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearInEntityContainsAdjustedMotto()
    {
        // Arrange
        MottoEntity motto = new() { ID = 1, Content = "d" };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = motto,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearInEntityContainsNonExistantPersonalEvent(int id)
    {
        // Arrange
        IList<PersonalEventEntity> personalEventEntities = new List<PersonalEventEntity>()
        {
            new()
            {
                ID = id,
                Title = "d",
            },
            new()
            {
                ID = 2,
                Title = DbEntitiesTestCases.SawTheMoon,
            },
        };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = personalEventEntities,
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearInEntityContainsAdjustedPersonalEvent()
    {
        // Arrange
        IList<PersonalEventEntity> personalEventEntities = new List<PersonalEventEntity>()
        {
            new()
            {
                ID = 1,
                Title = "d",
            },
            new()
            {
                ID = 2,
                Title = DbEntitiesTestCases.SawTheMoon,
            },
        };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = personalEventEntities,
                WorldEvents = new List<WorldEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearInEntityContainsNonExistantWorldEvent(int id)
    {
        // Arrange
        IList<WorldEventEntity> worldEventEntities = new List<WorldEventEntity>()
        {
            new()
            {
                ID = id,
                Title = "d",
            },
            new()
            {
                ID = 2,
                Title = DbEntitiesTestCases.SawTheMoon,
            },
        };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = worldEventEntities,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearInEntityContainsAdjustedWorldEvent()
    {
        // Arrange
        IList<WorldEventEntity> worldEventEntities = new List<WorldEventEntity>()
        {
            new()
            {
                ID = 1,
                Title = "d",
            },
            new()
            {
                ID = 2,
                Title = DbEntitiesTestCases.SawTheMoon,
            },
        };
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
                WorldEvents = worldEventEntities,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearInCommand);

        // Assert
        isValid.Should().BeFalse();
    }
}
