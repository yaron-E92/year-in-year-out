using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Sources;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.Sources;

internal class AddSourceCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddSourceCommandHandler _addSourceCommandHandler;
    private Mock<DbSet<SourceEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<SourceEntity>());

        _addSourceCommandHandler = new AddSourceCommandHandler(_dbContextMock.Object);
    }

    private void InitializeDbSet(IList<SourceEntity> sourceEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(sourceEntities);
        _dbContextMock.Setup(mock => mock.Sources)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Sources))]
    public async Task Should_PopulateCorrectSourceInContext_When_ValidAndNonExisting(SourceEntity sourceEntity)
    {
        // Arrange
        int originalId = sourceEntity.ID; // For mocking the db generating id
        AddSourceCommand addSourceCommand = new() { SourceEntity = new SourceEntity
        {
            ID = 0, // Indicate non existing
            Url = sourceEntity.Url,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.Sources.Single(m => m.Url.Equals(sourceEntity.Url)).ID = originalId); // Mocking id generation

        // Act
        SourceEntity source = await _addSourceCommandHandler.Handle(addSourceCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(m => m.Equals(source));
        _dbSetMock.Verify(dsm => dsm.AddAsync(source, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        source.ID.Should().BeGreaterThan(0);
        source.Url.Should().Be(sourceEntity.Url);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Sources))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingSource(SourceEntity sourceEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.Sources);
        AddSourceCommand addSourceCommand = new() { SourceEntity = sourceEntity };

        // Act
        SourceEntity source = await _addSourceCommandHandler.Handle(addSourceCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(m => m.Equals(sourceEntity));
        source.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullSource()
    {
        // Arrange
        AddSourceCommand addSourceCommand = new() { SourceEntity = null! };

        // Act
        SourceEntity source = await _addSourceCommandHandler.Handle(addSourceCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        source.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
    {
        // Arrange
        AddSourceCommand addSourceCommand = new()
        {
            SourceEntity = new SourceEntity
            {
                ID = id,
                Url = "https://source.net",
            },
        };

        // Act
        SourceEntity source = await _addSourceCommandHandler.Handle(addSourceCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        source.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        SourceEntity source = await _addSourceCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        source.Should().BeNull();
    }
}
