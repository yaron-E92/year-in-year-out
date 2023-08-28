using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries.Feelings;

public class GetFeelingQuery : IRequest<FeelingEntity>
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class GetFeelingQueryHandler : IRequestHandler<GetFeelingQuery, FeelingEntity>
{
    private readonly IApplicationDbContext _context;

    private IQueryable<FeelingEntity> FeelingsQueryable =>
        _context.Feelings.Include(e => e.PersonalEvents).AsNoTracking();

    public GetFeelingQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeelingEntity> Handle(GetFeelingQuery request, CancellationToken cancellationToken = default)
    {
        if (request.Id > 0)
        {
            return (await FeelingsQueryable
                .SingleOrDefaultAsync(
                    feel => feel.ID.Equals(request.Id), cancellationToken))!;
        }

        return (!string.IsNullOrWhiteSpace(request.Title)
            ? await FeelingsQueryable
                .SingleOrDefaultAsync(feel => feel.Title!.Equals(request.Title),
                    cancellationToken)
            : null)!;
    }
}
