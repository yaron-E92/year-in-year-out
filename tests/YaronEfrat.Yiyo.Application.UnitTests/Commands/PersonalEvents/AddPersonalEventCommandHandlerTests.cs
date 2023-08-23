using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.PersonalEvents;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.PersonalEvents;

internal class AddPersonalEventCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddPersonalEventCommandHandler _addPersonalEventCommandHandler;
    private Mock<DbSet<PersonalEventEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<PersonalEventEntity>());

        _addPersonalEventCommandHandler = new AddPersonalEventCommandHandler(_dbContextMock.Object,
            new PersonalEventDbEntityToDomainEntityMapper(),
            new PersonalEventDomainEntityToDbEntityMapper(),
            new CommandValidator<PersonalEventEntity>());
    }

    private void InitializeDbSet(IList<PersonalEventEntity> personalEventEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(personalEventEntities);
        _dbContextMock.Setup(mock => mock.PersonalEvents)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.PersonalEvents))]
    public async Task Should_PopulateCorrectPersonalEventInContext_When_ValidAndNonExisting(PersonalEventEntity personalEventEntity)
    {
        // Arrange
        int originalId = personalEventEntity.ID; // For mocking the db generating id
        AddPersonalEventCommand addPersonalEventCommand = new() { PersonalEventEntity = new PersonalEventEntity
        {
            ID = 0, // Indicate non existing
            Title = personalEventEntity.Title,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.PersonalEvents.Single(f => f.Title.Equals(personalEventEntity.Title.Trim())).ID = originalId); // Mocking id generation

        // Act
        PersonalEventEntity personalEvent = await _addPersonalEventCommandHandler.Handle(addPersonalEventCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(personalEvent));
        _dbSetMock.Verify(dsm => dsm.AddAsync(personalEvent, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        personalEvent.ID.Should().BeGreaterThan(0);
        personalEvent.Title.Should().Be(personalEventEntity.Title?.Trim());
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.PersonalEvents))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingPersonalEvent(PersonalEventEntity personalEventEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.PersonalEvents);
        AddPersonalEventCommand addPersonalEventCommand = new() { PersonalEventEntity = personalEventEntity };

        // Act
        PersonalEventEntity personalEvent = await _addPersonalEventCommandHandler.Handle(addPersonalEventCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(personalEventEntity));
        personalEvent.Should().BeNull();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowPersonalEventException_When_NonValidPersonalEvent(string invalidPersonalEventTitle)
    {
        // Arrange
        PersonalEventEntity personalEventEntity = new()
        {
            Title = invalidPersonalEventTitle,
        };
        AddPersonalEventCommand addPersonalEventCommand = new() { PersonalEventEntity = personalEventEntity };

        // Act
        Func<Task> handleCommandAction = async () => await _addPersonalEventCommandHandler.Handle(addPersonalEventCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(PersonalEvent)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullPersonalEvent()
    {
        // Arrange
        AddPersonalEventCommand addPersonalEventCommand = new() { PersonalEventEntity = null! };

        // Act
        PersonalEventEntity personalEvent = await _addPersonalEventCommandHandler.Handle(addPersonalEventCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        personalEvent.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
    {
        // Arrange
        AddPersonalEventCommand addPersonalEventCommand = new()
        {
            PersonalEventEntity = new PersonalEventEntity
            {
                ID = id,
                Title = "d",
            },
        };

        // Act
        PersonalEventEntity personalEvent = await _addPersonalEventCommandHandler.Handle(addPersonalEventCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        personalEvent.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        PersonalEventEntity personalEvent = await _addPersonalEventCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        personalEvent.Should().BeNull();
    }
}
