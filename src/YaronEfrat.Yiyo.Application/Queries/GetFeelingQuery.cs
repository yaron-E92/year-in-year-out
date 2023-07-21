using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetFeelingQuery : IRequest<FeelingEntity>
{
    public int Id { get; init; }

    public string Title { get; init; }
}

public class GetFeelingQueryHandler : IRequestHandler<GetFeelingQuery, FeelingEntity>
{
    private readonly IApplicationDbContext _context;

    public GetFeelingQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeelingEntity> Handle(GetFeelingQuery request, CancellationToken cancellationToken = default)
    {
        if (request.Id > 0)
        {
            return _context.Feelings.SingleOrDefault(feel => feel.ID.Equals(request.Id))!;
        }

        return (!string.IsNullOrWhiteSpace(request.Title)
            ? _context.Feelings.SingleOrDefault(feel => feel.Title.Equals(request.Title))
            : null)!;
    }
}
