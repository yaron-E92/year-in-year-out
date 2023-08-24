using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries.WorldEvents;

public class GetWorldEventQuery : IRequest<WorldEventEntity>
{
    public int Id { get; init; }

    public string? Title { get; init; }
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
            return (await _context.WorldEvents.SingleOrDefaultAsync(we => we.ID.Equals(request.Id),
                cancellationToken))!;
        }

        return (!string.IsNullOrWhiteSpace(request.Title)
            ? await _context.WorldEvents.SingleOrDefaultAsync(we => we.Title!.Equals(request.Title), cancellationToken)
            : null)!;
    }
}
