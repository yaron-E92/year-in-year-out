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
    private readonly bool _isChildCommand;

    public UpdateSourceCommandHandler(IApplicationDbContext context, bool isChildCommand = false)
    {
        _isChildCommand = isChildCommand;
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
        if (existingSourceEntity == null!)
        {
            return null!;
        }

        existingSourceEntity.Url = sourceEntity.Url;
        if (!_isChildCommand)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        return existingSourceEntity;
    }
}
