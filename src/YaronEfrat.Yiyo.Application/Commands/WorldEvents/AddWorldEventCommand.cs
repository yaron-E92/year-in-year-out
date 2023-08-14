using MediatR;

using YaronEfrat.Yiyo.Application.Commands.Sources;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Commands.WorldEvents;

public class AddWorldEventCommand : IRequest<WorldEventEntity>
{
    public WorldEventEntity WorldEventEntity { get; set; } = default!;
}

public class AddWorldEventCommandHandler : IRequestHandler<AddWorldEventCommand, WorldEventEntity>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent> _dbToDomainMapper;
    private readonly IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity> _domainToDbMapper;
    private readonly AddSourceCommandHandler _addSourceCommandHandler;
    private readonly UpdateSourceCommandHandler _updateSourceCommandHandler;

    public AddWorldEventCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity> domainToDbMapper)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
        _addSourceCommandHandler = new AddSourceCommandHandler(context, true);
        _updateSourceCommandHandler = new UpdateSourceCommandHandler(context, true);
    }

    public async Task<WorldEventEntity> Handle(AddWorldEventCommand request, CancellationToken cancellationToken = default)
    {
        if (request == null!)
        {
            return null!;
        }

        WorldEventEntity worldEventEntity = request.WorldEventEntity;
        if (worldEventEntity is not { ID: 0 })
        {
            return null!;
        }

        WorldEvent domainWorldEvent = _dbToDomainMapper.Map(worldEventEntity);
        domainWorldEvent.Validate();

        _domainToDbMapper.Map(domainWorldEvent, worldEventEntity);
        await HandleSources(worldEventEntity.Sources);
        await _context.WorldEvents.AddAsync(worldEventEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return worldEventEntity;
    }

    private async Task HandleSources(IEnumerable<SourceEntity> sources)
    {
        IList<Task<SourceEntity>> tasks = sources.Select(sourceEntity => sourceEntity.ID == 0
                ? AddSource(sourceEntity)
                : UpdateSource(sourceEntity))
            .ToList();
        await Task.WhenAll(tasks);
    }

    private Task<SourceEntity> AddSource(SourceEntity sourceEntity)
    {
        return _addSourceCommandHandler.Handle(new AddSourceCommand {SourceEntity = sourceEntity});
    }

    private Task<SourceEntity> UpdateSource(SourceEntity sourceEntity)
    {
        return _updateSourceCommandHandler.Handle(new UpdateSourceCommand {SourceEntity = sourceEntity});
    }
}
