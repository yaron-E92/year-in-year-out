using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Commands.Sources;

public class UpdateSourceCommand : IRequest<SourceEntity>
{
    public SourceEntity SourceEntity { get; set; } = default!;
}

public class UpdateSourceCommandHandler : IRequestHandler<UpdateSourceCommand, SourceEntity>
{
    private readonly IApplicationDbContext _context;

    public UpdateSourceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SourceEntity> Handle(UpdateSourceCommand request, CancellationToken cancellationToken = default)
    {
        if (request == null!)
        {
            return null!;
        }

        SourceEntity sourceEntity = request.SourceEntity;
        if (sourceEntity is { ID: <=0 } or null)
        {
            return null!;
        }

        SourceEntity existingSourceEntity = _context.Sources.SingleOrDefault(s => s.ID == sourceEntity.ID, null!);
        if (existingSourceEntity == null)
        {
            return null!;
        }

        existingSourceEntity.Url = sourceEntity.Url;
        await _context.SaveChangesAsync(cancellationToken);
        return existingSourceEntity;
    }
}
