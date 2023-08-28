using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
using YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

namespace YaronEfrat.Yiyo.Application.Commands.YearOuts;

public class AddYearOutCommand : IRequest<YearOutEntity>
{
    public YearOutEntity YearOutEntity { get; set; } = default!;
}

public class AddYearOutCommandHandler : IRequestHandler<AddYearOutCommand, YearOutEntity>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbEntityToDomainEntityMapper<YearOutEntity, YearOut> _dbToDomainMapper;
    private readonly IDomainEntityToDbEntityMapper<YearOut, YearOutEntity> _domainToDbMapper;

    private readonly ICommandValidator<YearOutEntity> _commandValidator;

    public AddYearOutCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<YearOutEntity, YearOut> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<YearOut, YearOutEntity> domainToDbMapper, ICommandValidator<YearOutEntity> commandValidator)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
        _commandValidator = commandValidator;
    }

    public async Task<YearOutEntity> Handle(AddYearOutCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _commandValidator.IsValidAddCommand(request))
        {
            return null!;
        }

        YearOutEntity dbEntity = request.YearOutEntity;
        YearOut domainEntity = _dbToDomainMapper.Map(dbEntity);
        domainEntity.Validate();

        _domainToDbMapper.Map(domainEntity, dbEntity);

        AttachRelations(dbEntity);

        await _context.YearOuts.AddAsync(dbEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return dbEntity;
    }

    private void AttachRelations(YearOutEntity yearOutEntity)
    {
        _context.Feelings.AttachRange(yearOutEntity.Feelings);
        if (yearOutEntity.Motto != null)
        {
            _context.Mottos.Attach(yearOutEntity.Motto);
        }
        _context.PersonalEvents.AttachRange(yearOutEntity.PersonalEvents);
    }
}
