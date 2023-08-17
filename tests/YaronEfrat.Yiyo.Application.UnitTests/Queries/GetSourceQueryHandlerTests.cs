using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetSourceQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetSourceQueryHandler _getSourceQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<SourceEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.Sources);
        _dbContextMock.Setup(mock => mock.Sources)
            .Returns(dbSetMock.Object);
        _getSourceQueryHandler = new GetSourceQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectSource_When_SearchingForExistingId(int id)
    {
        // Act
        SourceEntity source = await _getSourceQueryHandler.Handle(new GetSourceQuery {Id = id});

        // Assert
        source.ID.Should().Be(id);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        SourceEntity source = await _getSourceQueryHandler.Handle(new GetSourceQuery { Id = id });

        // Assert
        source.Should().BeNull();
    }
}
