using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Interfaces;

public interface IApplicationDbContext
{
    IDbConnection Connection { get; }

    DatabaseFacade Database { get; }

    DbSet<FeelingEntity> Feelings { get; }

    bool HasChanges { get; }

    DbSet<MottoEntity> Mottos { get; }

    DbSet<PersonalEventEntity> PersonalEvents { get; }

    DbSet<SourceEntity> Sources { get; }

    DbSet<WorldEventEntity> WorldEvents { get; }

    DbSet<YearInEntity> YearIns { get; }

    DbSet<YearOutEntity> YearOuts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
