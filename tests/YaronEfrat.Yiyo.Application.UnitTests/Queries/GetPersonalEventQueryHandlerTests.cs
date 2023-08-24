using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetPersonalEventQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetPersonalEventQueryHandler _getPersonalEventQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<PersonalEventEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.PersonalEvents);
        _dbContextMock.Setup(mock => mock.PersonalEvents)
            .Returns(dbSetMock.Object);
        _getPersonalEventQueryHandler = new GetPersonalEventQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectPersonalEvent_When_SearchingForExistingId(int id)
    {
        // Act
        PersonalEventEntity personalEvent = await _getPersonalEventQueryHandler.Handle(new GetPersonalEventQuery {Id = id});

        // Assert
        personalEvent.ID.Should().Be(id);
    }

    [TestCase(DbEntitiesTestCases.MovedToBerlin, Ignore = "Currently there is a bug since it has whitespace in the query. Remove ignore when fixed")]
    [TestCase(DbEntitiesTestCases.SawTheMoon)]
    public async Task Should_ReturnCorrectPersonalEvent_When_SearchingForExistingTitle(string title)
    {
        // Act
        PersonalEventEntity personalEvent = await _getPersonalEventQueryHandler.Handle(new GetPersonalEventQuery { Title = title });

        // Assert
        personalEvent.Title.Should().Be(title);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        PersonalEventEntity personalEvent = await _getPersonalEventQueryHandler.Handle(new GetPersonalEventQuery { Id = id });

        // Assert
        personalEvent.Should().BeNull();
    }

    [TestCase("Nothing")]
    [TestCase("")]
    public async Task Should_ReturnNull_When_SearchingForNonExistantTitle(string title)
    {
        // Act
        PersonalEventEntity personalEvent = await _getPersonalEventQueryHandler.Handle(new GetPersonalEventQuery { Title = title });

        // Assert
        personalEvent.Should().BeNull();
    }
}
