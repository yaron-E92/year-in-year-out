using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetFeelingQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetFeelingQueryHandler _getFeelingQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<FeelingEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.Feelings);
        _dbContextMock.Setup(mock => mock.Feelings)
            .Returns(dbSetMock.Object);
        _getFeelingQueryHandler = new GetFeelingQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectFeeling_When_SearchingForExistingId(int id)
    {
        // Act
        FeelingEntity feeling = await _getFeelingQueryHandler.Handle(new GetFeelingQuery {Id = id});

        // Assert
        feeling.ID.Should().Be(id);
    }

    [TestCase("Happy")]
    [TestCase("Sad")]
    public async Task Should_ReturnCorrectFeeling_When_SearchingForExistingTitle(string title)
    {
        // Act
        FeelingEntity feeling = await _getFeelingQueryHandler.Handle(new GetFeelingQuery { Title = title });

        // Assert
        feeling.Title.Should().Be(title);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        FeelingEntity feeling = await _getFeelingQueryHandler.Handle(new GetFeelingQuery { Id = id });

        // Assert
        feeling.Should().BeNull();
    }

    [TestCase("Nothing")]
    [TestCase("")]
    public async Task Should_ReturnNull_When_SearchingForNonExistantTitle(string title)
    {
        // Act
        FeelingEntity feeling = await _getFeelingQueryHandler.Handle(new GetFeelingQuery { Title = title });

        // Assert
        feeling.Should().BeNull();
    }
}
