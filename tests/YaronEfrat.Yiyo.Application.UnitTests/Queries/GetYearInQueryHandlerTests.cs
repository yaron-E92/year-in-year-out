using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetYearInQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetYearInQueryHandler _getYearInQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<YearInEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.YearIns);
        _dbContextMock.Setup(mock => mock.YearIns)
            .Returns(dbSetMock.Object);
        _getYearInQueryHandler = new GetYearInQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectYearIn_When_SearchingForExistingId(int id)
    {
        // Act
        YearInEntity yearIn = await _getYearInQueryHandler.Handle(new GetYearInQuery {Id = id});

        // Assert
        yearIn.ID.Should().Be(id);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        YearInEntity yearIn = await _getYearInQueryHandler.Handle(new GetYearInQuery { Id = id });

        // Assert
        yearIn.Should().BeNull();
    }
}
