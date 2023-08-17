using MediatR;

using Microsoft.EntityFrameworkCore;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetSourceQuery : IRequest<SourceEntity>
{
    public int Id { get; init; }
}

public class GetSourceQueryHandler : IRequestHandler<GetSourceQuery, SourceEntity>
{
    private readonly IApplicationDbContext _context;

    public GetSourceQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SourceEntity> Handle(GetSourceQuery request, CancellationToken cancellationToken = default)
    {
        return (await _context.Sources.SingleOrDefaultAsync(source => source.ID.Equals(request.Id),
            cancellationToken))!;
    }
}
