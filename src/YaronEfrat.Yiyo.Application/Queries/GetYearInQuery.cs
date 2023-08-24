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

    public GetYearInQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<YearInEntity> Handle(GetYearInQuery request, CancellationToken cancellationToken = default)
    {
        return (await _context.YearIns.AsNoTracking()
            .SingleOrDefaultAsync(yearIn => yearIn.ID.Equals(request.Id),
            cancellationToken))!;
    }
}
