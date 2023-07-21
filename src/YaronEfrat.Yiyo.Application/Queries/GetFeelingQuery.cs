using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetFeelingQuery : IRequest<FeelingEntity>
{
    public int Id { get; init; }
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
        return _context.Feelings.SingleOrDefault(feel => feel.ID.Equals(request.Id));
    }
}
