using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Commands.YearIns;

public class AddYearInCommand : IRequest<YearInEntity>
{
    public YearInEntity YearInEntity { get; set; } = default!;
}

public class AddYearInCommandHandler : IRequestHandler<AddYearInCommand, YearInEntity>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbEntityToDomainEntityMapper<YearInEntity, YearIn> _dbToDomainMapper;
    private readonly IDomainEntityToDbEntityMapper<YearIn, YearInEntity> _domainToDbMapper;

    public AddYearInCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<YearInEntity, YearIn> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<YearIn, YearInEntity> domainToDbMapper)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
    }

    public async Task<YearInEntity> Handle(AddYearInCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.IsValidAddCommand())
        {
            return null!;
        }

        YearInEntity yearInEntity = request.YearInEntity;
        YearIn yearIn = _dbToDomainMapper.Map(yearInEntity);
        yearIn.Validate();

        _domainToDbMapper.Map(yearIn, yearInEntity);

        await _context.YearIns.AddAsync(yearInEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return yearInEntity;
    }
}
