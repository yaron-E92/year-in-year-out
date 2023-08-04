using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetYearOutQuery : IRequest<YearOutEntity>
{
    public int Id { get; init; }
}

public class GetYearOutQueryHandler : IRequestHandler<GetYearOutQuery, YearOutEntity>
{
    private readonly IApplicationDbContext _context;

    public GetYearOutQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<YearOutEntity> Handle(GetYearOutQuery request, CancellationToken cancellationToken = default)
    {
        return (await _context.YearOuts.SingleOrDefaultAsync(yearOut => yearOut.ID.Equals(request.Id),
            cancellationToken))!;
    }
}
