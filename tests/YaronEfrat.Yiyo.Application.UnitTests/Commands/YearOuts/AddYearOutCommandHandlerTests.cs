using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.YearOuts;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.Feelings;
using YaronEfrat.Yiyo.Application.Mappers.Mottos;
using YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;
using YaronEfrat.Yiyo.Application.Mappers.YearOuts;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.YearOuts;

internal class AddYearOutCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddYearOutCommandHandler _addYearOutCommandHandler;
    private Mock<DbSet<YearOutEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<YearOutEntity>());

        PersonalEventDbEntityToDomainEntityMapper personalEventDbToDomainMapper = new();
        YearOutDbEntityToDomainEntityMapper dbToDomainMapper = new(
            new FeelingDbEntityToDomainEntityMapper(personalEventDbToDomainMapper),
            new MottoDbEntityToDomainEntityMapper(),
            personalEventDbToDomainMapper);
        PersonalEventDomainEntityToDbEntityMapper personalEventDomainToDbMapper = new();
        YearOutDomainEntityToDbEntityMapper domainToDbMapper = new(
            new FeelingDomainEntityToDbEntityMapper(personalEventDomainToDbMapper),
            new MottoDomainEntityToDbEntityMapper(),
            personalEventDomainToDbMapper);
        _addYearOutCommandHandler = new AddYearOutCommandHandler(_dbContextMock.Object,
            dbToDomainMapper, domainToDbMapper,
            new CommandValidator<YearOutEntity>(null!));
    }

    private void InitializeDbSet(IList<YearOutEntity> yearOutEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(yearOutEntities);
        _dbContextMock.Setup(mock => mock.YearOuts)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.YearOuts))]
    public async Task Should_PopulateCorrectYearOutInContext_When_ValidAndNonExisting(YearOutEntity yearOutEntity)
    {
        // Arrange
        int originalId = yearOutEntity.ID; // For mocking the db generating id
        AddYearOutCommand addYearOutCommand = new() { YearOutEntity = new YearOutEntity
        {
            ID = 0, // Indicate non existing
            Motto = yearOutEntity.Motto,
            Feelings = yearOutEntity.Feelings,
            PersonalEvents = yearOutEntity.PersonalEvents,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.YearOuts.Single(yo => yo.Motto.Content.Equals(yearOutEntity.Motto.Content.Trim())).ID = originalId); // Mocking id generation

        // Act
        YearOutEntity yearOut = await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(yo => yo.Equals(yearOut));
        _dbSetMock.Verify(dsm => dsm.AddAsync(yearOut, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        yearOut.ID.Should().BeGreaterThan(0);
        yearOut.Motto.Should().Be(yearOutEntity.Motto);
        yearOut.Feelings.Should().BeEquivalentTo(yearOutEntity.Feelings);
        yearOut.PersonalEvents.Should().BeEquivalentTo(yearOutEntity.PersonalEvents);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.YearOuts))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingYearOut(YearOutEntity yearOutEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.YearOuts);
        AddYearOutCommand addYearOutCommand = new() { YearOutEntity = yearOutEntity };

        // Act
        YearOutEntity yearOut = await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(yo => yo.Equals(yearOutEntity));
        yearOut.Should().BeNull();
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.InvalidFeelings))]
    public async Task Should_ThrowYearOutException_When_NonValidFeeling(FeelingEntity feelingEntity)
    {
        // Arrange
        AddYearOutCommand addYearOutCommand = new() { YearOutEntity = new YearOutEntity
        {
            ID = 0,
            Feelings = new List<FeelingEntity> { feelingEntity },
            Motto = new MottoEntity {ID = 1, Content = "d"},
            PersonalEvents = DbEntitiesTestCases.PersonalEvents,
        } };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(Feeling)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowYearOutException_When_NonValidMotto(string invalidMottoContent)
    {
        // Arrange
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = DbEntitiesTestCases.Feelings,
                Motto = new MottoEntity { ID = 1, Content = invalidMottoContent },
                PersonalEvents = DbEntitiesTestCases.PersonalEvents,
            },
        };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(Motto)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowYearOutException_When_NonValidPersonalEvent(string invalidPersonalEventTitle)
    {
        // Arrange
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = 0,
                Feelings = DbEntitiesTestCases.Feelings,
                Motto = new MottoEntity { ID = 1, Content = "d" },
                PersonalEvents = new List<PersonalEventEntity> { new() { ID = 1, Title = invalidPersonalEventTitle } },
            },
        };

        // Act
        Func<Task> handleCommandAction = async () => await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(PersonalEvent)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullYearOut()
    {
        // Arrange
        AddYearOutCommand addYearOutCommand = new() { YearOutEntity = null! };

        // Act
        YearOutEntity yearOut = await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        yearOut.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
    {
        // Arrange
        AddYearOutCommand addYearOutCommand = new()
        {
            YearOutEntity = new YearOutEntity
            {
                ID = id,
            },
        };

        // Act
        YearOutEntity yearOut = await _addYearOutCommandHandler.Handle(addYearOutCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        yearOut.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        YearOutEntity yearOut = await _addYearOutCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        yearOut.Should().BeNull();
    }
}
