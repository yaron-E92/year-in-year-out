using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace YaronEfrat.Yiyo.Application.Interfaces;

public interface IApplicationDbContext
{
    IDbConnection Connection { get; }

    DatabaseFacade Database { get; }

    DbSet<FeelingEntity> Feelings { get; }

    bool HasChanges { get; }

    DbSet<MottoEntity> Mottos { get; }

    DbSet<PersonalEventEntity> PersonalEvents { get; }

    DbSet<WorldEventEntity> WorldEvents { get; }

    DbSet<YearInEntity> YearIns { get; }

    DbSet<YearOutEntity> YearOuts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
