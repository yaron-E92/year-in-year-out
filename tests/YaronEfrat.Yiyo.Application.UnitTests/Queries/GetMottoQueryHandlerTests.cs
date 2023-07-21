using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Moq;

using NUnit.Framework;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;

namespace YaronEfrat.Yiyo.Application.UnitTests.Queries;

internal class GetMottoQueryHandlerTests
{
    private Mock<IApplicationDbContext> _dbContextMock;

    private GetMottoQueryHandler _getMottoQueryHandler;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IApplicationDbContext>();

        Mock<DbSet<MottoEntity>> dbSetMock = TestFixtures.DbSetMock(DbEntitiesTestCases.Mottos);
        _dbContextMock.Setup(mock => mock.Mottos)
            .Returns(dbSetMock.Object);
        _getMottoQueryHandler = new GetMottoQueryHandler(_dbContextMock.Object);
    }

    [TestCase(1)]
    [TestCase(2)]
    public async Task Should_ReturnCorrectFeeling_When_SearchingForExistingId(int id)
    {
        // Act
        MottoEntity feeling = await _getMottoQueryHandler.Handle(new GetMottoQuery {Id = id});

        // Assert
        feeling.ID.Should().Be(id);
    }

    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(100000)]
    public async Task Should_ReturnNull_When_SearchingForNonExistantId(int id)
    {
        // Act
        MottoEntity feeling = await _getMottoQueryHandler.Handle(new GetMottoQuery { Id = id });

        // Assert
        feeling.Should().BeNull();
    }
}
