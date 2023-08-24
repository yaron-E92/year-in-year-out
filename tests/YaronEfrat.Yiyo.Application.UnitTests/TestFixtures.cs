using Microsoft.EntityFrameworkCore;

using MockQueryable.Moq;

using Moq;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

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

    internal static FeelingEntity Clone(FeelingEntity feelingEntity)
    {
        return new FeelingEntity
        {
            ID = feelingEntity.ID,
            Description = feelingEntity.Description,
            PersonalEvents = feelingEntity.PersonalEvents.Select(Clone).ToList(),
            Title = feelingEntity.Title,
        };
    }

    internal static PersonalEventEntity Clone(PersonalEventEntity personalEventEntity)
    {
        return new PersonalEventEntity
        {
            ID = personalEventEntity.ID,
            Title = personalEventEntity.Title,
        };
    }

    private static SourceEntity Clone(SourceEntity sourceEntity)
    {
        return new SourceEntity
        {
            ID = sourceEntity.ID,
            Url = sourceEntity.Url,
        };
    }

    internal static WorldEventEntity Clone(WorldEventEntity worldEventEntity)
    {
        return new WorldEventEntity
        {
            ID = worldEventEntity.ID,
            Title = worldEventEntity.Title,
            Sources = worldEventEntity.Sources.Select(Clone).ToList(),
        };
    }
}
