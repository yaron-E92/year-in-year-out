using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries.WorldEvents;

public class GetWorldEventListQuery : IRequest<WorldEventEntity[]>
{
    public IReadOnlySet<int> Ids { get; init; } = default!;
}

public class GetWorldEventListQueryHandler : IRequestHandler<GetWorldEventListQuery, WorldEventEntity[]>
{
    private readonly IApplicationDbContext _context;

    public GetWorldEventListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorldEventEntity[]> Handle(GetWorldEventListQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.Ids == null!)
        {
            return await _context.WorldEvents.ToArrayAsync(cancellationToken);
        }

        // TODO [#18]: add query validator to validate all ids are positive
        return await _context.WorldEvents
            .Where(pe => request.Ids.Contains(pe.ID))
            .ToArrayAsync(cancellationToken);
    }
}
