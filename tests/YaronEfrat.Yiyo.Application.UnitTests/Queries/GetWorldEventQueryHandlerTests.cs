using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetWorldEventQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetWorldEventQueryHandler _getWorldEventQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<WorldEventEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.WorldEvents);
        _dbContextMock.Setup(mock => mock.WorldEvents)
            .Returns(dbSetMock.Object);
        _getWorldEventQueryHandler = new GetWorldEventQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectWorldEvent_When_SearchingForExistingId(int id)
    {
        // Act
        WorldEventEntity worldEvent = await _getWorldEventQueryHandler.Handle(new GetWorldEventQuery {Id = id});

        // Assert
        worldEvent.ID.Should().Be(id);
    }

    [TestCase(DbEntitiesTestCases.Corona, Ignore = "Currently there is a bug since it has two matches. Remove ignore when fixed")]
    [TestCase(DbEntitiesTestCases.War)]
    public async Task Should_ReturnCorrectWorldEvent_When_SearchingForExistingTitle(string title)
    {
        // Act
        WorldEventEntity worldEvent = await _getWorldEventQueryHandler.Handle(new GetWorldEventQuery { Title = title });

        // Assert
        worldEvent.Title.Should().Be(title);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        WorldEventEntity worldEvent = await _getWorldEventQueryHandler.Handle(new GetWorldEventQuery { Id = id });

        // Assert
        worldEvent.Should().BeNull();
    }

    [TestCase("Nothing")]
    [TestCase("")]
    public async Task Should_ReturnNull_When_SearchingForNonExistantTitle(string title)
    {
        // Act
        WorldEventEntity worldEvent = await _getWorldEventQueryHandler.Handle(new GetWorldEventQuery { Title = title });

        // Assert
        worldEvent.Should().BeNull();
    }
}
