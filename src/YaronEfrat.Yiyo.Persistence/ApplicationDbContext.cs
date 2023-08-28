using System.Data;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public IDbConnection Connection => Database.GetDbConnection();
    public DbSet<FeelingEntity> Feelings { get; set; } = default!;
    public bool HasChanges => ChangeTracker.HasChanges();
    public DbSet<MottoEntity> Mottos { get; set; } = default!;
    public DbSet<PersonalEventEntity> PersonalEvents { get; set; } = default!;
    public DbSet<SourceEntity> Sources { get; set; } = default!;
    public DbSet<WorldEventEntity> WorldEvents { get; set; } = default!;
    public DbSet<YearInEntity> YearIns { get; set; } = default!;
    public DbSet<YearOutEntity> YearOuts { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeelingEntity>()
            .HasMany(e => e.PersonalEvents)
            .WithMany();

        modelBuilder.Entity<WorldEventEntity>()
            .HasMany(e => e.Sources)
            .WithMany();
    }
}
