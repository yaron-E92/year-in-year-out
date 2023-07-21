using MediatR;

using YaronEfrat.Yiyo.Application.Interfaces;
using YaronEfrat.Yiyo.Application.Models;

namespace YaronEfrat.Yiyo.Application.Queries;

public class GetMottoQuery : IRequest<MottoEntity>
{
    public int Id { get; init; }
}

public class GetMottoQueryHandler : IRequestHandler<GetMottoQuery, MottoEntity>
{
    private readonly IApplicationDbContext _context;

    public GetMottoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MottoEntity> Handle(GetMottoQuery request, CancellationToken cancellationToken = default)
    {
        return _context.Mottos.SingleOrDefault(motto => motto.ID.Equals(request.Id))!;
    }
}
