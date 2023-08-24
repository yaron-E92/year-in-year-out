using FluentAssertions;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Feelings;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;
using YaronEfrat.Yiyo.Application.Validators;

namespace YaronEfrat.Yiyo.Application.UnitTests.Validators;
internal class FeelingCommandValidatorTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private FeelingCommandValidator _validator;

    private Mock<IMediator> _mediatorMock;

    private GetPersonalEventListQueryHandler _getPersonalEventListQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<PersonalEventEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.PersonalEvents);
        _dbContextMock.Setup(mock => mock.PersonalEvents)
            .Returns(dbSetMock.Object);

        _getPersonalEventListQueryHandler = new GetPersonalEventListQueryHandler(_dbContextMock.Object);

        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonalEventListQuery>(), default))
            .Returns(async (GetPersonalEventListQuery q, CancellationToken token) =>
                await _getPersonalEventListQueryHandler.Handle(q, token));

        _validator = new FeelingCommandValidator(_mediatorMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Feelings))]
    public async Task Should_ReturnTrue_When_ValidCommand(FeelingEntity feelingEntity)
    {
        // Arrange
        AddFeelingCommand addFeelingCommand = new()
        {
            FeelingEntity = new FeelingEntity
            {
                ID = 0, // Indicate non existing
                Title = feelingEntity.Title,
                Description = feelingEntity.Description,
                PersonalEvents = feelingEntity.PersonalEvents.Select(TestFixtures.Clone).ToList(),
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addFeelingCommand);

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
        AddFeelingCommand addFeelingCommand = new()
        {
            FeelingEntity = new FeelingEntity
            {
                ID = id,
                Title = "d",
                Description = "s",
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addFeelingCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [TestCase(-1)]
    [TestCase(61)]
    [TestCase(0)]
    public async Task Should_ReturnFalse_When_FeelingEntityContainsNonExistantPersonalEvent(int id)
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
        AddFeelingCommand addFeelingCommand = new()
        {
            FeelingEntity = new FeelingEntity
            {
                ID = 0,
                Title = "d",
                Description = "s",
                PersonalEvents = personalEventEntities,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addFeelingCommand);

        // Assert
        isValid.Should().BeFalse();
    }

    [Test]
    public async Task Should_ReturnFalse_When_FeelingEntityContainsAdjustedPersonalEvent()
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
        AddFeelingCommand addFeelingCommand = new()
        {
            FeelingEntity = new FeelingEntity
            {
                ID = 0,
                Title = "d",
                Description = "s",
                PersonalEvents = personalEventEntities,
            },
        };

        // Act
        bool isValid = await _validator.IsValidAddCommand(addFeelingCommand);

        // Assert
        isValid.Should().BeFalse();
    }
}
