using FluentAssertions;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.YearOuts;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;
using YaronEfrat.Yiyo.Application.Queries.Feelings;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;
using YaronEfrat.Yiyo.Application.Validators;

namespace YaronEfrat.Yiyo.Application.UnitTests.Validators;
internal class YearOutCommandValidatorTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private YearOutCommandValidator _validator;

    private Mock<IMediator> _mediatorMock;

    private GetFeelingListQueryHandler _getFeelingListQueryHandler;
    private GetMottoQueryHandler _getMottoQueryHandler;
    private GetPersonalEventListQueryHandler _getPersonalEventListQueryHandler;

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

        _getFeelingListQueryHandler = new GetFeelingListQueryHandler(_dbContextMock.Object);
        _getMottoQueryHandler = new GetMottoQueryHandler(_dbContextMock.Object);
        _getPersonalEventListQueryHandler = new GetPersonalEventListQueryHandler(_dbContextMock.Object);

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

        _validator = new YearOutCommandValidator(_mediatorMock.Object,
            new Mock<ILogger<YearOutCommandValidator>>().Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.YearOuts))]
    public async Task Should_ReturnTrue_When_ValidCommand(YearOutEntity yearOutEntity)
    {
        // Arrange
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0, // Indicate non existing
                Feelings = yearOutEntity.Feelings.Select(TestFixtures.Clone).ToList(),
                Motto = yearOutEntity.Motto,
                PersonalEvents = yearOutEntity.PersonalEvents.Select(TestFixtures.Clone).ToList(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

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
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = id,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearOutEntityContainsNonExistantFeeling(int id)
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
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = feelingEntities,
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearOutEntityContainsAdjustedFeeling()
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
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = feelingEntities,
                Motto = null,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearOutEntityContainsNonExistantMotto(int id)
    {
        // Arrange
        MottoEntity motto = new() {ID = id, Content = "d"};
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = motto,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearOutEntityContainsAdjustedMotto()
    {
        // Arrange
        MottoEntity motto = new() { ID = 1, Content = "d" };
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = motto,
                PersonalEvents = new List<PersonalEventEntity>(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_YearOutEntityContainsNonExistantPersonalEvent(int id)
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
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = personalEventEntities,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_YearOutEntityContainsAdjustedPersonalEvent()
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
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = new List<FeelingEntity>(),
                Motto = null,
                PersonalEvents = personalEventEntities,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addYearOutCommand);

        // Assert
        isValid.Should().BeFalse();
    }
}
