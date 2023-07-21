using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetPersonalEventQuery : IRequest<PersonalEventEntity>
{
    public int Id { get; init; }

    public string Title { get; init; }
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
            return _context.PersonalEvents.SingleOrDefault(pe => pe.ID.Equals(request.Id))!;
        }

        return (!string.IsNullOrWhiteSpace(request.Title)
            ? _context.PersonalEvents.SingleOrDefault(pe => pe.Title.Equals(request.Title))
            : null)!;
    }
}
