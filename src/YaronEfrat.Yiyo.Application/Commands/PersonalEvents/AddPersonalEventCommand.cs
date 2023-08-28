using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Commands.PersonalEvents;

public class AddPersonalEventCommand : IRequest<PersonalEventEntity>
{
    public PersonalEventEntity PersonalEventEntity { get; set; } = default!;
}

public class AddPersonalEventCommandHandler : IRequestHandler<AddPersonalEventCommand, PersonalEventEntity>
{
    private readonly IApplicationDbContext _context;

    private readonly IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> _dbToDomainMapper;
    private readonly IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> _domainToDbMapper;

    private readonly ICommandValidator<PersonalEventEntity> _commandValidator;

    public AddPersonalEventCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> domainToDbMapper,
        ICommandValidator<PersonalEventEntity> commandValidator)
    {
        _domainToDbMapper = domainToDbMapper;
        _commandValidator = commandValidator;
        _dbToDomainMapper = dbToDomainMapper;
        _context = context;
    }

    public async Task<PersonalEventEntity> Handle(AddPersonalEventCommand request,
        CancellationToken cancellationToken = default)
    {
        if (!await _commandValidator.IsValidAddCommand(request))
        {
            return null!;
        }

        PersonalEventEntity dbEntity = request.PersonalEventEntity;
        PersonalEvent domainEntity = _dbToDomainMapper.Map(dbEntity);
        domainEntity.Validate();

        _domainToDbMapper.Map(domainEntity, dbEntity);
        await _context.PersonalEvents.AddAsync(dbEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return dbEntity;
    }
}
