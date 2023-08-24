using MediatR;

using Microsoft.AspNetCore.Mvc;

using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries.WorldEvents;
using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.WebApi.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class WorldEventController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WorldEventController> _logger;

    public WorldEventController(IMediator mediator, ILogger<WorldEventController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<WorldEventEntity>> Get([FromRoute]int id)
    {
        try
        {
            WorldEventEntity worldEventEntity = await _mediator.Send(new GetWorldEventQuery {Id = id});
            return worldEventEntity != null! ? Ok(worldEventEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
