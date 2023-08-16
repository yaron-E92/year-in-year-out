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
    private readonly IMediator _mediator;

    public AddWorldEventCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity> domainToDbMapper,
        IMediator mediator)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
        _mediator = mediator;
    }

    public async Task<WorldEventEntity> Handle(AddWorldEventCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.IsValidAddCommand())
        {
            return null!;
        }

        WorldEventEntity worldEventEntity = request.WorldEventEntity;
        WorldEvent domainWorldEvent = _dbToDomainMapper.Map(worldEventEntity);
        domainWorldEvent.Validate();

        _domainToDbMapper.Map(domainWorldEvent, worldEventEntity);
        await HandleSources(worldEventEntity.Sources, cancellationToken);
        await _context.WorldEvents.AddAsync(worldEventEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return worldEventEntity;
    }

    private async Task HandleSources(IEnumerable<SourceEntity> sources, CancellationToken cancellationToken)
    {
        IList<Task<SourceEntity>> tasks = sources.Select(sourceEntity => sourceEntity.ID == 0
                ? AddSource(sourceEntity, cancellationToken)
                : UpdateSource(sourceEntity, cancellationToken))
            .ToList();
        await Task.WhenAll(tasks);
    }

    private Task<SourceEntity> AddSource(SourceEntity sourceEntity, CancellationToken cancellationToken)
    {
        return _mediator.Send(new AddSourceCommand
            {
                SourceEntity = sourceEntity,
                IsChildCommand = true,
            },
            cancellationToken);
    }

    private Task<SourceEntity> UpdateSource(SourceEntity sourceEntity, CancellationToken cancellationToken)
    {
        return _mediator.Send(new UpdateSourceCommand
            {
                SourceEntity = sourceEntity,
                IsChildCommand = true,
            },
            cancellationToken);
    }
}
