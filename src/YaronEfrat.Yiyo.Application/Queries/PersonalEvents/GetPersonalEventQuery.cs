using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries.PersonalEvents;

public class GetPersonalEventQuery : IRequest<PersonalEventEntity>
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class GetPersonalEventQueryHandler : IRequestHandler<GetPersonalEventQuery, PersonalEventEntity>
{
    private readonly IApplicationDbContext _context;

    public GetPersonalEventQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PersonalEventEntity> Handle(GetPersonalEventQuery request, CancellationToken cancellationToken = default)
    {
        if (request.Id > 0)
        {
            return (await _context.PersonalEvents.AsNoTracking()
                .SingleOrDefaultAsync(pe => pe.ID.Equals(request.Id),
                cancellationToken))!;
        }

        return (!string.IsNullOrWhiteSpace(request.Title)
            ? await _context.PersonalEvents.AsNoTracking()
                .SingleOrDefaultAsync(pe => pe.Title!.Equals(request.Title),
                cancellationToken)
            : null)!;
    }
}
