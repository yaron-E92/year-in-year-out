using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Application.UnitTests;

internal class TestFixtures
{
    internal static Mock<DbSet<T>> DbSetMock<T>(IList<T> sourceList) where T : class, IDbEntity
    {
        Mock<DbSet<T>> dbSetMock = sourceList.AsQueryable().BuildMockDbSet();
        dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
        dbSetMock.Setup(d => d.AddAsync(It.IsAny<T>(), default)).Callback<T, CancellationToken>((fe, _) => sourceList.Add(fe));
        return dbSetMock;
    }
}
