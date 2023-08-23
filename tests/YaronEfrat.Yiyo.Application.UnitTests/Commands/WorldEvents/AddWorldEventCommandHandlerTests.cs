using FluentAssertions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Sources;
using YaronEfrat.Yiyo.Application.Commands.WorldEvents;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.WorldEvents;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.WorldEvents;

internal class AddWorldEventCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddWorldEventCommandHandler _addWorldEventCommandHandler;
    private Mock<DbSet<WorldEventEntity>> _dbSetMock;
    private Mock<DbSet<SourceEntity>> _sourcesMock;
    private Mock<IMediator> _mediatorMock;

    private AddSourceCommandHandler _addSourceCommandHandler;
    private UpdateSourceCommandHandler _updateSourceCommandHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<WorldEventEntity>());

        _sourcesMock = TestFixtures.DbSetMock(DbEntitiesTestCases.Sources);
        _dbContextMock.Setup(mock => mock.Sources)
            .Returns(_sourcesMock.Object);

        _addSourceCommandHandler = new AddSourceCommandHandler(_dbContextMock.Object,
            new CommandValidator<SourceEntity>());
        _updateSourceCommandHandler = new UpdateSourceCommandHandler(_dbContextMock.Object);

        _mediatorMock = new Mock<IMediator>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<AddSourceCommand>(), default))
            .Returns(async (AddSourceCommand c, CancellationToken token) =>
                await _addSourceCommandHandler.Handle(c, token));
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateSourceCommand>(), default))
            .Returns(async (UpdateSourceCommand c, CancellationToken token) =>
                await _updateSourceCommandHandler.Handle(c, token));

        _addWorldEventCommandHandler = new AddWorldEventCommandHandler(_dbContextMock.Object,
            new WorldEventDbEntityToDomainEntityMapper(),
            new WorldEventDomainEntityToDbEntityMapper(),
            _mediatorMock.Object,
            new CommandValidator<WorldEventEntity>());
    }

    private void InitializeDbSet(IList<WorldEventEntity> worldEventEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(worldEventEntities);
        _dbContextMock.Setup(mock => mock.WorldEvents)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.WorldEvents))]
    public async Task Should_PopulateCorrectWorldEventInContext_When_ValidAndNonExisting(WorldEventEntity worldEventEntity)
    {
        // Arrange
        int originalId = worldEventEntity.ID; // For mocking the db generating id
        AddWorldEventCommand addWorldEventCommand = new() { WorldEventEntity = new WorldEventEntity
        {
            ID = 0, // Indicate non existing
            Title = worldEventEntity.Title,
            Sources = worldEventEntity.Sources,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
        {
            _dbContextMock.Object.WorldEvents.Single(we => we.Title.Equals(worldEventEntity.Title.Trim())).ID =
                originalId;
            SourceEntity sourceEntity = _dbContextMock.Object.Sources.SingleOrDefault(s => s.ID == 0);
            if (sourceEntity != null)
            {
                sourceEntity.ID = 3; // To avoid an invalid source in other tests
            }
        }); // Mocking id generation

        // Act
        WorldEventEntity worldEvent = await _addWorldEventCommandHandler.Handle(addWorldEventCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(w => w.Equals(worldEvent));
        _sourcesMock.Object.Should().BeEquivalentTo(worldEventEntity.Sources);
        _dbSetMock.Verify(dsm => dsm.AddAsync(worldEvent, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        worldEvent.ID.Should().BeGreaterThan(0);
        worldEvent.Title.Should().Be(worldEventEntity.Title?.Trim());
        worldEvent.Sources.Should().BeEquivalentTo(worldEventEntity.Sources);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.WorldEvents))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingWorldEvent(WorldEventEntity worldEventEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.WorldEvents);
        AddWorldEventCommand addWorldEventCommand = new() { WorldEventEntity = worldEventEntity };

        // Act
        WorldEventEntity worldEvent = await _addWorldEventCommandHandler.Handle(addWorldEventCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(worldEventEntity));
        worldEvent.Should().BeNull();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowWorldEventException_When_NonValidWorldEvent(string invalidWorldEventTitle)
    {
        // Arrange
        WorldEventEntity worldEventEntity = new()
        {
            Title = invalidWorldEventTitle,
        };
        AddWorldEventCommand addWorldEventCommand = new() { WorldEventEntity = worldEventEntity };

        // Act
        Func<Task> handleCommandAction = async () => await _addWorldEventCommandHandler.Handle(addWorldEventCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(WorldEvent)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullWorldEvent()
    {
        // Arrange
        AddWorldEventCommand addWorldEventCommand = new() { WorldEventEntity = null! };

        // Act
        WorldEventEntity worldEvent = await _addWorldEventCommandHandler.Handle(addWorldEventCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        worldEvent.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
    {
        // Arrange
        AddWorldEventCommand addWorldEventCommand = new()
        {
            WorldEventEntity = new WorldEventEntity
            {
                ID = id,
                Title = "d",
            },
        };

        // Act
        WorldEventEntity worldEvent = await _addWorldEventCommandHandler.Handle(addWorldEventCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        worldEvent.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        WorldEventEntity worldEvent = await _addWorldEventCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        worldEvent.Should().BeNull();
    }
}
