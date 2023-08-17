using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
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

    public AddPersonalEventCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<PersonalEventEntity, PersonalEvent> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<PersonalEvent, PersonalEventEntity> domainToDbMapper)
    {
        _domainToDbMapper = domainToDbMapper;
        _dbToDomainMapper = dbToDomainMapper;
        _context = context;
    }

    public async Task<PersonalEventEntity> Handle(AddPersonalEventCommand request,
        CancellationToken cancellationToken = default)
    {
        if (!request.IsValidAddCommand())
        {
            return null!;
        }

        PersonalEventEntity personalEventEntity = request.PersonalEventEntity;
        PersonalEvent domainPersonalEvent = _dbToDomainMapper.Map(personalEventEntity);
        domainPersonalEvent.Validate();

        _domainToDbMapper.Map(domainPersonalEvent, personalEventEntity);
        await _context.PersonalEvents.AddAsync(personalEventEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return personalEventEntity;
    }
}
