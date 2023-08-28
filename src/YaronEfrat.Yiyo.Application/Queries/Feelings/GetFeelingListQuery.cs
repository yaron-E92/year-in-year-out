using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries.Feelings;

public class GetFeelingListQuery : IRequest<FeelingEntity[]>
{
    public IReadOnlySet<int> Ids { get; init; } = default!;
}

public class GetFeelingListQueryHandler : IRequestHandler<GetFeelingListQuery, FeelingEntity[]>
{
    private readonly IApplicationDbContext _context;

    public GetFeelingListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeelingEntity[]> Handle(GetFeelingListQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.Ids == null!)
        {
            return await _context.Feelings.AsNoTracking().ToArrayAsync(cancellationToken);
        }

        // TODO [#18]: add query validator to validate all ids are positive
        return await _context.Feelings.AsNoTracking()
            .Where(pe => request.Ids.Contains(pe.ID))
            .ToArrayAsync(cancellationToken);
    }
}
