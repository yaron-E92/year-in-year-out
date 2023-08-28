using FluentAssertions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Feelings;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.Feelings;
using YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.Feelings;

internal class AddFeelingCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddFeelingCommandHandler _addFeelingCommandHandler;
    private Mock<DbSet<FeelingEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<FeelingEntity>());

        _addFeelingCommandHandler = new AddFeelingCommandHandler(_dbContextMock.Object,
            new FeelingDbEntityToDomainEntityMapper(new PersonalEventDbEntityToDomainEntityMapper()),
            new FeelingDomainEntityToDbEntityMapper(new PersonalEventDomainEntityToDbEntityMapper()),
            new CommandValidator<FeelingEntity>(null!,
                new Mock<ILogger<CommandValidator<FeelingEntity>>>().Object));
    }

    private void InitializeDbSet(IList<FeelingEntity> feelingEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(feelingEntities);
        _dbContextMock.Setup(mock => mock.Feelings)
            .Returns(_dbSetMock.Object);
        _dbContextMock.Setup(mock => mock.PersonalEvents)
            .Returns(TestFixtures.DbSetMock(DbEntitiesTestCases.PersonalEvents).Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Feelings))]
    public async Task Should_PopulateCorrectFeelingInContext_When_ValidAndNonExisting(FeelingEntity feelingEntity)
    {
        // Arrange
        int originalId = feelingEntity.ID; // For mocking the db generating id
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = new FeelingEntity
        {
            ID = 0, // Indicate non existing
            Title = feelingEntity.Title,
            Description = feelingEntity.Description,
            PersonalEvents = feelingEntity.PersonalEvents,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.Feelings.Single(f => f.Title.Equals(feelingEntity.Title.Trim())).ID = originalId); // Mocking id generation

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(feeling));
        _dbSetMock.Verify(dsm => dsm.AddAsync(feeling, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        feeling.ID.Should().BeGreaterThan(0);
        feeling.Title.Should().Be(feelingEntity.Title?.Trim());
        feeling.Description.Should().Be(feelingEntity.Description?.Trim());
        feeling.PersonalEvents.Should().BeEquivalentTo(feelingEntity.PersonalEvents);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Feelings))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingFeeling(FeelingEntity feelingEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.Feelings);
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = feelingEntity };

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(feelingEntity));
        feeling.Should().BeNull();
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.InvalidFeelings))]
    public async Task Should_ThrowFeelingException_When_NonValidFeeling(FeelingEntity feelingEntity)
    {
        // Arrange
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = feelingEntity };

        // Act
        Func<Task> handleCommandAction = async () => await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(Feeling)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullFeeling()
    {
        // Arrange
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = null! };

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        feeling.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
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
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        feeling.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        feeling.Should().BeNull();
    }
}
