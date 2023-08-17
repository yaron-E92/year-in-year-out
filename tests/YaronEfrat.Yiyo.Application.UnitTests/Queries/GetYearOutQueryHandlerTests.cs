using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetYearOutQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetYearOutQueryHandler _getYearOutQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<YearOutEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.YearOuts);
        _dbContextMock.Setup(mock => mock.YearOuts)
            .Returns(dbSetMock.Object);
        _getYearOutQueryHandler = new GetYearOutQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectYearOut_When_SearchingForExistingId(int id)
    {
        // Act
        YearOutEntity yearOut = await _getYearOutQueryHandler.Handle(new GetYearOutQuery {Id = id});

        // Assert
        yearOut.ID.Should().Be(id);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        YearOutEntity yearOut = await _getYearOutQueryHandler.Handle(new GetYearOutQuery { Id = id });

        // Assert
        yearOut.Should().BeNull();
    }
}
