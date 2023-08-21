using MediatR;

using Microsoft.AspNetCore.Mvc;

using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;
using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.WebApi.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class SourceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SourceController> _logger;

    public SourceController(IMediator mediator, ILogger<SourceController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<SourceEntity>> Get([FromRoute]int id)
    {
        try
        {
            SourceEntity sourceEntity = await _mediator.Send(new GetSourceQuery {Id = id});
            return sourceEntity != null! ? Ok(sourceEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
