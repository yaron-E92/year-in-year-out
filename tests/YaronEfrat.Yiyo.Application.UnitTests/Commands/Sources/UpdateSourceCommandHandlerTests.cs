using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Sources;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.Sources;

internal class UpdateSourceCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private UpdateSourceCommandHandler _updateSourceCommandHandler;
    private Mock<DbSet<SourceEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(DbEntitiesTestCases.Sources);

        _updateSourceCommandHandler = new UpdateSourceCommandHandler(_dbContextMock.Object);
    }

    private void InitializeDbSet(IList<SourceEntity> sourceEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(sourceEntities);
        _dbContextMock.Setup(mock => mock.Sources)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Sources))]
    public async Task Should_UpdateCorrectSourceInContext_When_Existing(SourceEntity sourceEntity)
    {
        // Arrange
        UpdateSourceCommand updateSourceCommand = new() { SourceEntity = new SourceEntity
        {
            ID = sourceEntity.ID, // Indicate non existing
            Url = DbEntitiesTestCases.NewUrl,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.Sources.Single(m => m.ID.Equals(sourceEntity.ID)).Url = DbEntitiesTestCases.NewUrl); // Mocking id generation

        // Act
        SourceEntity source = await _updateSourceCommandHandler.Handle(updateSourceCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(m => m.Equals(source));
        _dbSetMock.Verify(dsm => dsm.AddAsync(source, default), Times.Never);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        source.ID.Should().Be(sourceEntity.ID);
        source.Url.Should().Be(DbEntitiesTestCases.NewUrl);
    }

    [TestCase(4)]
    [TestCase(-1)]
    [TestCase(0)]
    public async Task Should_DoNothingAndReturnNull_When_NonExistingSource(int id)
    {
        // Arrange
        SourceEntity sourceEntity = new() { ID = id };
        UpdateSourceCommand updateSourceCommand = new() { SourceEntity = sourceEntity };

        // Act
        SourceEntity source = await _updateSourceCommandHandler.Handle(updateSourceCommand);

        // Assert
        _dbSetMock.Object.Should().NotContain(m => m.Equals(sourceEntity));
        source.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullSource()
    {
        // Arrange
        UpdateSourceCommand updateSourceCommand = new() { SourceEntity = null! };

        // Act
        SourceEntity source = await _updateSourceCommandHandler.Handle(updateSourceCommand);

        // Assert
        _dbSetMock.Object.Should().BeEquivalentTo(DbEntitiesTestCases.Sources);
        source.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        SourceEntity source = await _updateSourceCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEquivalentTo(DbEntitiesTestCases.Sources);
        source.Should().BeNull();
    }
}
