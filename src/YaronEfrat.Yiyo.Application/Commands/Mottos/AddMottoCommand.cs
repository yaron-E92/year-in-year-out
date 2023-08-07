using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Commands.Mottos;

public class AddMottoCommand : IRequest<MottoEntity>
{
    public MottoEntity MottoEntity { get; set; } = default!;
}

public class AddMottoCommandHandler : IRequestHandler<AddMottoCommand, MottoEntity>
{
    private readonly IApplicationDbContext _context;

    private readonly IDbEntityToDomainEntityMapper<MottoEntity, Motto> _dbToDomainMapper;
    private readonly IDomainEntityToDbEntityMapper<Motto, MottoEntity> _domainToDbMapper;

    public AddMottoCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<MottoEntity, Motto> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<Motto, MottoEntity> domainToDbMapper)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
    }

    public async Task<MottoEntity> Handle(AddMottoCommand request, CancellationToken cancellationToken = default)
    {
        if (request == null!)
        {
            return null!;
        }

        MottoEntity mottoEntity = request.MottoEntity;
        if (mottoEntity is not { ID: 0 })
        {
            return null!;
        }

        Motto domainMotto = _dbToDomainMapper.Map(mottoEntity);
        domainMotto.Validate();

        _domainToDbMapper.Map(domainMotto, mottoEntity);
        await _context.Mottos.AddAsync(mottoEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return mottoEntity;
    }
}
