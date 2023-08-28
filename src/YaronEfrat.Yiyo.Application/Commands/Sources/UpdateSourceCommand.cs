using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Commands.Sources;

public class UpdateSourceCommand : IRequest<SourceEntity>
{
    public SourceEntity SourceEntity { get; set; } = default!;

    public bool IsChildCommand { get; init; } = false;
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

        SourceEntity dbEntity = request.SourceEntity;
        if (dbEntity is { ID: <=0 } or null)
        {
            return null!;
        }

        SourceEntity existingDbEntity = _context.Sources.SingleOrDefault(s => s.ID == dbEntity.ID, null!);
        if (existingDbEntity == null!)
        {
            return null!;
        }

        existingDbEntity.Url = dbEntity.Url;
        if (!request.IsChildCommand)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return existingDbEntity;
    }
}
