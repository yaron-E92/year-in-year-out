using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Validators;

namespace YaronEfrat.Yiyo.Application.Commands.Sources;

public class AddSourceCommand : IRequest<SourceEntity>
{
    public SourceEntity SourceEntity { get; set; } = default!;

    public bool IsChildCommand { get; init; } = false;
}

public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand, SourceEntity>
{
    private readonly IApplicationDbContext _context;

    private readonly ICommandValidator<SourceEntity> _commandValidator;

    public AddSourceCommandHandler(IApplicationDbContext context, ICommandValidator<SourceEntity> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<SourceEntity> Handle(AddSourceCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _commandValidator.IsValidAddCommand(request))
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
