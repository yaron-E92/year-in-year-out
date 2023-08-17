using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Commands.Sources;

public class AddSourceCommand : IRequest<SourceEntity>
{
    public SourceEntity SourceEntity { get; set; } = default!;

    public bool IsChildCommand { get; init; } = false;
}

public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand, SourceEntity>
{
    private readonly IApplicationDbContext _context;

    public AddSourceCommandHandler(IApplicationDbContext context)
    {
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
        if (!request.IsChildCommand)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return sourceEntity;
    }
}
