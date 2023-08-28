using MediatR;

using YaronEfrat.Yiyo.Application.Commands.Sources;
using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
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

    private readonly ICommandValidator<WorldEventEntity> _commandValidator;
    private readonly IMediator _mediator;

    public AddWorldEventCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<WorldEventEntity, WorldEvent> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<WorldEvent, WorldEventEntity> domainToDbMapper,
        IMediator mediator,
        ICommandValidator<WorldEventEntity> commandValidator)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
        _mediator = mediator;
        _commandValidator = commandValidator;
    }

    public async Task<WorldEventEntity> Handle(AddWorldEventCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _commandValidator.IsValidAddCommand(request))
        {
            return null!;
        }

        WorldEventEntity dbEntity = request.WorldEventEntity;
        WorldEvent domainEntity = _dbToDomainMapper.Map(dbEntity);
        domainEntity.Validate();

        _domainToDbMapper.Map(domainEntity, dbEntity);
        await HandleSources(dbEntity.Sources, cancellationToken);
        await _context.WorldEvents.AddAsync(dbEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return dbEntity;
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
