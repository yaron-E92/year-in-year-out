using MediatR;

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

    public AddSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SourceEntity> Handle(AddSourceCommand request, CancellationToken cancellationToken = default)
    {
        if (request == null!)
        {
            return null!;
        }

        SourceEntity sourceEntity = request.SourceEntity;
        if (sourceEntity is not { ID: 0 })
        {
            return null!;
        }

        await _context.Sources.AddAsync(sourceEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sourceEntity;
    }
}
