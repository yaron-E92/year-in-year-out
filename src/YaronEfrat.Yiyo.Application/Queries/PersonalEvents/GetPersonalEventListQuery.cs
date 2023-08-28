using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries.PersonalEvents;

public class GetPersonalEventListQuery : IRequest<PersonalEventEntity[]>
{
    public IReadOnlySet<int> Ids { get; init; } = default!;
}

public class GetPersonalEventListQueryHandler : IRequestHandler<GetPersonalEventListQuery, PersonalEventEntity[]>
{
    private readonly IApplicationDbContext _context;

    public GetPersonalEventListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PersonalEventEntity[]> Handle(GetPersonalEventListQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.Ids == null!)
        {
            return await _context.PersonalEvents.AsNoTracking().ToArrayAsync(cancellationToken);
        }

        // TODO [#18]: add query validator to validate all ids are positive
        return await _context.PersonalEvents.AsNoTracking()
            .Where(pe => request.Ids.Contains(pe.ID))
            .ToArrayAsync(cancellationToken);
    }
}
