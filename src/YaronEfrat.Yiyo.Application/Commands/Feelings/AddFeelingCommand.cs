using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
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

    public AddFeelingCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<FeelingEntity, Feeling> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<Feeling, FeelingEntity> domainToDbMapper)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
    }

    public async Task<FeelingEntity> Handle(AddFeelingCommand request, CancellationToken cancellationToken = default)
    {
        if (request == null!)
        {
            return null!;
        }

        FeelingEntity feelingEntity = request.FeelingEntity;
        if (feelingEntity is not {ID: 0})
        {
            return null!;
        }

        Feeling domainFeeling = _dbToDomainMapper.Map(feelingEntity);
        domainFeeling.Validate();

        _domainToDbMapper.Map(domainFeeling, feelingEntity);
        await _context.Feelings.AddAsync(feelingEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return feelingEntity;
    }
}
