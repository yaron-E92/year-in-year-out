using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetYearInQuery : IRequest<YearInEntity>
{
    public int Id { get; init; }
}

public class GetYearInQueryHandler : IRequestHandler<GetYearInQuery, YearInEntity>
{
    private readonly IApplicationDbContext _context;

    private IQueryable<YearInEntity> YearInQueryable =>
        _context.YearIns
            .Include(e => e.Feelings)
            .Include(e => e.Motto)
            .Include(e => e.PersonalEvents)
            .Include(e => e.WorldEvents)
            .AsSplitQuery()
            .AsNoTracking();

    public GetYearInQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<YearInEntity> Handle(GetYearInQuery request, CancellationToken cancellationToken = default)
    {
        return (await YearInQueryable
            .SingleOrDefaultAsync(yearIn => yearIn.ID.Equals(request.Id),
            cancellationToken))!;
    }
}
