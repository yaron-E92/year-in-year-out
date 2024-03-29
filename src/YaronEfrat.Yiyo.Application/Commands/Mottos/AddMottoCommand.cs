﻿using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;
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

    private readonly ICommandValidator<MottoEntity> _commandValidator;

    public AddMottoCommandHandler(IApplicationDbContext context,
        IDbEntityToDomainEntityMapper<MottoEntity, Motto> dbToDomainMapper,
        IDomainEntityToDbEntityMapper<Motto, MottoEntity> domainToDbMapper,
        ICommandValidator<MottoEntity> commandValidator)
    {
        _context = context;
        _dbToDomainMapper = dbToDomainMapper;
        _domainToDbMapper = domainToDbMapper;
        _commandValidator = commandValidator;
    }

    public async Task<MottoEntity> Handle(AddMottoCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _commandValidator.IsValidAddCommand(request))
        {
            return null!;
        }

        MottoEntity dbEntity = request.MottoEntity;
        Motto domainEntity = _dbToDomainMapper.Map(dbEntity);
        domainEntity.Validate();

        _domainToDbMapper.Map(domainEntity, dbEntity);
        await _context.Mottos.AddAsync(dbEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return dbEntity;
    }
}
