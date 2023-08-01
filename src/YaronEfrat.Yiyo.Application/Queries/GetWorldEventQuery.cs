using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetWorldEventQuery : IRequest<WorldEventEntity>
{
    public int Id { get; init; }

    public string Title { get; init; }
}

public class GetWorldEventQueryHandler : IRequestHandler<GetWorldEventQuery, WorldEventEntity>
{
    private readonly IApplicationDbContext _context;

    public GetWorldEventQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorldEventEntity> Handle(GetWorldEventQuery request, CancellationToken cancellationToken = default)
    {
        if (request.Id > 0)
        {
            return _context.WorldEvents.SingleOrDefault(pe => pe.ID.Equals(request.Id))!;
        }

        return (!string.IsNullOrWhiteSpace(request.Title)
            ? _context.WorldEvents.SingleOrDefault(pe => pe.Title.Equals(request.Title))
            : null)!;
    }
}
