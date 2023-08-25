using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Commands.Feelings;
public class AddFeelingCommand : IRequest<FeelingEntity>
{
    public FeelingEntity FeelingEntity { get; set; } = default!;
}

public class AddFeelingCommandHandler : IRequestHandler<AddFeelingCommand, FeelingEntity>
{
    private readonly IApplicationDbContext _context;

    private readonly IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> _dbToDomainMapper;
    private readonly IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> _domainToDbMapper;

    private readonly ICommandValidator<FeelingEntity> _commandValidator;

    public AddFeelingCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> domainToDbMapper,
        ICommandValidator<FeelingEntity> commandValidator)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
        _commandValidator = commandValidator;
    }

    public async Task<FeelingEntity> Handle(AddFeelingCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _commandValidator.IsValidAddCommand(request))
        {
            return null!;
        }

        FeelingEntity feelingEntity = request.FeelingEntity;
        Feeling domainFeeling = _dbToDomainMapper.Map(feelingEntity);
        domainFeeling.Validate();

        _domainToDbMapper.Map(domainFeeling, feelingEntity);

        _context.PersonalEvents.AttachRange(feelingEntity.PersonalEvents);

        await _context.Feelings.AddAsync(feelingEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return feelingEntity;
    }
}
