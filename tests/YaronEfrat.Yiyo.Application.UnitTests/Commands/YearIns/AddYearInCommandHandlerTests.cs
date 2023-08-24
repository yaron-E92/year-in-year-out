using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.YearIns;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.Feelings;
using YaronEfrat.Yiyo.Application.Mappers.Mottos;
using YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;
using YaronEfrat.Yiyo.Application.Mappers.WorldEvents;
using YaronEfrat.Yiyo.Application.Mappers.YearIns;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.YearIns;

internal class AddYearInCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddYearInCommandHandler _addYearInCommandHandler;
    private Mock<DbSet<YearInEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<YearInEntity>());

        PersonalEventDbEntityToDomainEntityMapper personalEventDbToDomainMapper = new();
        YearInDbEntityToDomainEntityMapper dbToDomainMapper = new(
            new FeelingDbEntityToDomainEntityMapper(personalEventDbToDomainMapper),
            new MottoDbEntityToDomainEntityMapper(),
            personalEventDbToDomainMapper,
            new WorldEventDbEntityToDomainEntityMapper());
        PersonalEventDomainEntityToDbEntityMapper personalEventDomainToDbMapper = new();
        YearInDomainEntityToDbEntityMapper domainToDbMapper = new(
            new FeelingDomainEntityToDbEntityMapper(personalEventDomainToDbMapper),
            new MottoDomainEntityToDbEntityMapper(),
            personalEventDomainToDbMapper,
            new WorldEventDomainEntityToDbEntityMapper());
        _addYearInCommandHandler = new AddYearInCommandHandler(_dbContextMock.Object,
            dbToDomainMapper, domainToDbMapper,
            new CommandValidator<YearInEntity>(null!));
    }

    private void InitializeDbSet(IList<YearInEntity> yearInEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(yearInEntities);
        _dbContextMock.Setup(mock => mock.YearIns)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.YearIns))]
    public async Task Should_PopulateCorrectYearInInContext_When_ValidAndNonExisting(YearInEntity yearInEntity)
    {
        // Arrange
        int originalId = yearInEntity.ID; // For mocking the db generating id
        AddYearInCommand addYearInCommand = new() { YearInEntity = new YearInEntity
        {
            ID = 0, // Indicate non existing
            Motto = yearInEntity.Motto,
            Feelings = yearInEntity.Feelings,
            PersonalEvents = yearInEntity.PersonalEvents,
            WorldEvents = yearInEntity.WorldEvents,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.YearIns.Single(yi => yi.Motto.Content.Equals(yearInEntity.Motto.Content.Trim())).ID = originalId); // Mocking id generation

        // Act
        YearInEntity yearIn = await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(yi => yi.Equals(yearIn));
        _dbSetMock.Verify(dsm => dsm.AddAsync(yearIn, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        yearIn.ID.Should().BeGreaterThan(0);
        yearIn.Motto.Should().Be(yearInEntity.Motto);
        yearIn.Feelings.Should().BeEquivalentTo(yearInEntity.Feelings);
        yearIn.PersonalEvents.Should().BeEquivalentTo(yearInEntity.PersonalEvents);
        yearIn.WorldEvents.Should().BeEquivalentTo(yearInEntity.WorldEvents);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.YearIns))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingYearIn(YearInEntity yearInEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.YearIns);
        AddYearInCommand addYearInCommand = new() { YearInEntity = yearInEntity };

        // Act
        YearInEntity yearIn = await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(yo => yo.Equals(yearInEntity));
        yearIn.Should().BeNull();
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.InvalidFeelings))]
    public async Task Should_ThrowYearInException_When_NonValidFeeling(FeelingEntity feelingEntity)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new() { YearInEntity = new YearInEntity
        {
            ID = 0,
            Feelings = new List<FeelingEntity> { feelingEntity },
            Motto = new MottoEntity {ID = 1, Content = "d"},
            PersonalEvents = DbEntitiesTestCases.PersonalEvents,
            WorldEvents = DbEntitiesTestCases.WorldEvents,
        } };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(Feeling)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowYearInException_When_NonValidMotto(string invalidMottoContent)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = DbEntitiesTestCases.Feelings,
                Motto = new MottoEntity { ID = 1, Content = invalidMottoContent },
                PersonalEvents = DbEntitiesTestCases.PersonalEvents,
                WorldEvents = DbEntitiesTestCases.WorldEvents,
            },
        };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(Motto)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowYearInException_When_NonValidPersonalEvent(string invalidPersonalEventTitle)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = DbEntitiesTestCases.Feelings,
                Motto = new MottoEntity { ID = 1, Content = "d" },
                PersonalEvents = new List<PersonalEventEntity> { new() { ID = 1, Title = invalidPersonalEventTitle } },
                WorldEvents = DbEntitiesTestCases.WorldEvents,
            },
        };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(PersonalEvent)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowYearInException_When_NonValidWorldEvent(string invalidWorldEventTitle)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = 0,
                Feelings = DbEntitiesTestCases.Feelings,
                Motto = new MottoEntity { ID = 1, Content = "d" },
                PersonalEvents = DbEntitiesTestCases.PersonalEvents,
                WorldEvents = new List<WorldEventEntity> { new() { ID = 1, Title = invalidWorldEventTitle } },
            },
        };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(WorldEvent)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullYearIn()
    {
        // Arrange
        AddYearInCommand addYearInCommand = new() { YearInEntity = null! };

        // Act
        YearInEntity yearIn = await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        yearIn.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
    {
        // Arrange
        AddYearInCommand addYearInCommand = new()
        {
            YearInEntity = new YearInEntity
            {
                ID = id,
            },
        };

        // Act
        YearInEntity yearIn = await _addYearInCommandHandler.Handle(addYearInCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        yearIn.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        YearInEntity yearIn = await _addYearInCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        yearIn.Should().BeNull();
    }
}
