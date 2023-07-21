using Microsoft.EntityFrameworkCore;

using Moq;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.UnitTests;

internal class TestFixtures
{
    internal static Mock<DbSet<T>> DbSetMock<T>(IReadOnlyList<T> sourceList) where T : class, IDbEntity
    {
        IQueryable<T> queryable = sourceList.AsQueryable();
        Mock<DbSet<T>> dbSetMock = new();
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        return dbSetMock;
    }
}
