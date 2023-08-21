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
public class FeelingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FeelingController> _logger;

    public FeelingController(IMediator mediator, ILogger<FeelingController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FeelingEntity>> Get([FromRoute]int id)
    {
        try
        {
            FeelingEntity feelingEntity = await _mediator.Send(new GetFeelingQuery {Id = id});
            return feelingEntity != null! ? Ok(feelingEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
