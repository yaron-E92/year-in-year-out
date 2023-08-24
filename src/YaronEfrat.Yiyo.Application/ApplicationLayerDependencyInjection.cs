using Microsoft.Extensions.DependencyInjection;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Mappers.Feelings;
using YaronEfrat.Yiyo.Application.Mappers.Mottos;
using YaronEfrat.Yiyo.Application.Mappers.PersonalEvents;
using YaronEfrat.Yiyo.Application.Mappers.WorldEvents;
using YaronEfrat.Yiyo.Application.Mappers.YearIns;
using YaronEfrat.Yiyo.Application.Mappers.YearOuts;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application;

public static class ApplicationLayerDependencyInjection
{
    public static void AddApplicationLayerDependencyInjection(this IServiceCollection services)
    {
        // Db to domain mappers
        services
            .AddScoped<IDbEntityToDomainEntityMapper<FeelingEntity, Feeling>,
                FeelingDbEntityToDomainEntityMapper>()
            .AddScoped<IDbEntityToDomainEntityMapper<MottoEntity, Motto>,
                MottoDbEntityToDomainEntityMapper>()
            .AddScoped<IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent>,
                PersonalEventDbEntityToDomainEntityMapper>()
            .AddScoped<IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent>,
                WorldEventDbEntityToDomainEntityMapper>()
            .AddScoped<IDbEntityToDomainEntityMapper<YearInEntity, YearIn>,
                YearInDbEntityToDomainEntityMapper>()
            .AddScoped<IDbEntityToDomainEntityMapper<YearOutEntity, YearOut>,
                YearOutDbEntityToDomainEntityMapper>();

        // Domain to db mappers
        services
            .AddScoped<IDomainEntityToDbEntityMapper<Feeling, FeelingEntity>,
                FeelingDomainEntityToDbEntityMapper>()
            .AddScoped<IDomainEntityToDbEntityMapper<Motto, MottoEntity>,
                MottoDomainEntityToDbEntityMapper>()
            .AddScoped<IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity>,
                PersonalEventDomainEntityToDbEntityMapper>()
            .AddScoped<IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity>,
                WorldEventDomainEntityToDbEntityMapper>()
            .AddScoped<IDomainEntityToDbEntityMapper<YearIn, YearInEntity>,
                YearInDomainEntityToDbEntityMapper>()
            .AddScoped<IDomainEntityToDbEntityMapper<YearOut, YearOutEntity>,
                YearOutDomainEntityToDbEntityMapper>();

        // Command validators
        services.AddScoped<ICommandValidator<FeelingEntity>, FeelingCommandValidator>()
            .AddScoped<ICommandValidator<MottoEntity>, CommandValidator<MottoEntity>>()
            .AddScoped<ICommandValidator<PersonalEventEntity>, CommandValidator<PersonalEventEntity>>()
            .AddScoped<ICommandValidator<SourceEntity>, CommandValidator<SourceEntity>>()
            .AddScoped<ICommandValidator<WorldEventEntity>, CommandValidator<WorldEventEntity>>()
            .AddScoped<ICommandValidator<YearInEntity>, YearInCommandValidator>()
            .AddScoped<ICommandValidator<YearOutEntity>, YearOutCommandValidator>();

        // Mediatr
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayerDependencyInjection).Assembly));
    }
}
