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
public class YearInController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<YearInController> _logger;

    public YearInController(IMediator mediator, ILogger<YearInController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<YearInEntity>> Get([FromRoute]int id)
    {
        try
        {
            YearInEntity yearInEntity = await _mediator.Send(new GetYearInQuery {Id = id});
            return yearInEntity != null! ? Ok(yearInEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
