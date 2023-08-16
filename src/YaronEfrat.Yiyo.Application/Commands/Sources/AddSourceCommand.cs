﻿using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Commands.Sources;

public class AddSourceCommand : IRequest<SourceEntity>
{
    public SourceEntity SourceEntity { get; set; } = default!;
}

public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand, SourceEntity>
{
    private readonly IApplicationDbContext _context;
    private readonly bool _isChildCommand;

    public AddSourceCommandHandler(IApplicationDbContext context, bool isChildCommand = false)
    {
        _isChildCommand = isChildCommand;
        _context = context;
    }

    public async Task<SourceEntity> Handle(AddSourceCommand request, CancellationToken cancellationToken = default)
    {
        if (!request.IsValidAddCommand())
        {
            return null!;
        }

        SourceEntity sourceEntity = request.SourceEntity;
        await _context.Sources.AddAsync(sourceEntity, cancellationToken);
        if (!_isChildCommand)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return sourceEntity;
    }
}
