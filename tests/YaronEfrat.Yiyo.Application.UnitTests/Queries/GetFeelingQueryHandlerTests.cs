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

        var queryable =  FeelingTestCases.Feelings.AsQueryable();
        var dbSetMock = new Mock<DbSet<FeelingEntity>>();
        dbSetMock.As<IQueryable<FeelingEntity>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<FeelingEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<FeelingEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<FeelingEntity>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
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
}
