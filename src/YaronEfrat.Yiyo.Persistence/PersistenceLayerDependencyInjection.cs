using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using YaronEfrat.Yiyo.Application.Interfaces;

namespace YaronEfrat.Yiyo.Persistence;

public static class PersistenceLayerDependencyInjection
{
    private const string DefaultConnection = "DefaultConnection";

    public static void AddPersistenceLayerDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(DefaultConnection)));

        services.AddScoped<IApplicationDbContext>(provider =>
            {
                var context = provider.GetService<ApplicationDbContext>() ??
                              throw new Exception("Could not get DB context.");
                context.Database.EnsureCreated();
                RelationalDatabaseCreator databaseCreator =
                    (RelationalDatabaseCreator) context.Database.GetService<IDatabaseCreator>();
                if (!databaseCreator.HasTables())
                {
                    databaseCreator.CreateTables();
                }
                return context;
            }
            );
    }
}
