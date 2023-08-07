using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Commands.Mottos;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.Mottos;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.UnitTests.Commands.Mottos;

internal class AddMottoCommandHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private AddMottoCommandHandler _addMottoCommandHandler;
    private Mock<DbSet<MottoEntity>> _dbSetMock;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        InitializeDbSet(new List<MottoEntity>());

        _addMottoCommandHandler = new AddMottoCommandHandler(_dbContextMock.Object,
            new MottoDbEntityToDomainEntityMapper(),
            new MottoDomainEntityToDbEntityMapper());
    }

    private void InitializeDbSet(IList<MottoEntity> mottoEntities)
    {
        _dbSetMock = TestFixtures.DbSetMock(mottoEntities);
        _dbContextMock.Setup(mock => mock.Mottos)
            .Returns(_dbSetMock.Object);
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Mottos))]
    public async Task Should_PopulateCorrectMottoInContext_When_ValidAndNonExisting(MottoEntity mottoEntity)
    {
        // Arrange
        int originalId = mottoEntity.ID; // For mocking the db generating id
        AddMottoCommand addMottoCommand = new() { MottoEntity = new MottoEntity
        {
            ID = 0, // Indicate non existing
            Content = mottoEntity.Content,
        } };
        _dbContextMock.Setup(cm => cm.SaveChangesAsync(default)).Callback(() =>
            _dbContextMock.Object.Mottos.Single(m => m.Content.Equals(mottoEntity.Content.Trim())).ID = originalId); // Mocking id generation

        // Act
        MottoEntity motto = await _addMottoCommandHandler.Handle(addMottoCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(m => m.Equals(motto));
        _dbSetMock.Verify(dsm => dsm.AddAsync(motto, default), Times.Once);
        _dbContextMock.Verify(cm => cm.SaveChangesAsync(default), Times.Once);
        motto.ID.Should().BeGreaterThan(0);
        motto.Content.Should().Be(mottoEntity.Content.Trim());
    }

    [TestCaseSource(typeof(DbEntitiesTestCases), nameof(DbEntitiesTestCases.Mottos))]
    public async Task Should_DoNothingAndReturnNull_When_ExistingMotto(MottoEntity mottoEntity)
    {
        // Arrange
        InitializeDbSet(DbEntitiesTestCases.Mottos);
        AddMottoCommand addMottoCommand = new() { MottoEntity = mottoEntity };

        // Act
        MottoEntity motto = await _addMottoCommandHandler.Handle(addMottoCommand);

        // Assert
        _dbSetMock.Object.Should().Contain(m => m.Equals(mottoEntity));
        motto.Should().BeNull();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task Should_ThrowMottoException_When_NonValidMotto(string invalidMottoContent)
    {
        // Arrange

        MottoEntity mottoEntity = new()
        {
            Content = invalidMottoContent,
        };
        AddMottoCommand addMottoCommand = new() { MottoEntity = mottoEntity };

        // Act
        Func<Task> handleCommandAction = async () => await _addMottoCommandHandler.Handle(addMottoCommand);

        // Assert
        await handleCommandAction.Should().ThrowAsync<EntityException>().WithMessage($"*{nameof(Motto)}*");
        _dbSetMock.Object.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullMotto()
    {
        // Arrange
        AddMottoCommand addMottoCommand = new() { MottoEntity = null! };

        // Act
        MottoEntity motto = await _addMottoCommandHandler.Handle(addMottoCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        motto.Should().BeNull();
    }

    [TestCase(-1)]
    [TestCase(61)]
    public async Task Should_ReturnNull_When_NonZeroId(int id)
    {
        // Arrange
        AddMottoCommand addMottoCommand = new()
        {
            MottoEntity = new MottoEntity
            {
                ID = id,
                Content = "s",
            },
        };

        // Act
        MottoEntity motto = await _addMottoCommandHandler.Handle(addMottoCommand);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        motto.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_NullCommand()
    {
        // Act
        MottoEntity motto = await _addMottoCommandHandler.Handle(null!);

        // Assert
        _dbSetMock.Object.Should().BeEmpty();
        motto.Should().BeNull();
    }
}
