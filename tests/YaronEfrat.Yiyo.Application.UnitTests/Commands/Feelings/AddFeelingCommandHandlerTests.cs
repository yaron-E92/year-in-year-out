using System.Collections.Generic;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Feelings;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

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

        InitializeDbSet(DbEntitiesTestCases.Feelings);
        _addFeelingCommandHandler = new AddFeelingCommandHandler(_dbContextMock.Object);
    }

    private void InitializeDbSet(IList<FeelingEntity> feelingEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(feelingEntities);
        _dbContextMock.Setup(mock => mock.Feelings)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Feelings))]
    public async Task Should_PopulateCorrectFeelingInContext_When_ValidAndNonExisting(FeelingEntity feelingEntity)
    {
        // Arrange
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = feelingEntity };

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(feelingEntity));
        feeling.Should().Be(feelingEntity);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Feelings))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingFeeling(FeelingEntity feelingEntity)
    {
        // Arrange
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = feelingEntity };

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(f => f.Equals(feelingEntity));
        feeling.Should().BeNull();
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.InvalidFeelings))]
    public async Task Should_ReturnNull_When_NonValidFeeling(FeelingEntity feelingEntity)
    {
        // Arrange
        AddFeelingCommand addFeelingCommand = new() { FeelingEntity = feelingEntity };

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().BeEquivalentTo(DbEntitiesTestCases.Feelings);
        feeling.Should().BeNull();
    }

    [TestCase(null)]
    public async Task Should_ReturnNull_When_NullCommand(AddFeelingCommand addFeelingCommand)
    {
        // Arrange
        InitializeDbSet(new List<FeelingEntity>());

        // Act
        FeelingEntity feeling = await _addFeelingCommandHandler.Handle(addFeelingCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        feeling.Should().BeNull();
    }
}
