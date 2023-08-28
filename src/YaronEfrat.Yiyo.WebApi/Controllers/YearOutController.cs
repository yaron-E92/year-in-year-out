using MediatR;

using Microsoft.AspNetCore.Mvc;

using YaronEfrat.Yiyo.Application.Commands.YearOuts;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;
using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.WebApi.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class YearOutController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<YearOutController> _logger;

    public YearOutController(IMediator mediator, ILogger<YearOutController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<YearOutEntity>> Get([FromRoute] int id)
    {
        try
        {
            YearOutEntity yearOutEntity = await _mediator.Send(new GetYearOutQuery { Id = id });
            return yearOutEntity != null! ? Ok(yearOutEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] YearOutEntity yearOutEntity)
    {
        try
        {
            yearOutEntity = await _mediator.Send(new AddYearOutCommand { YearOutEntity = yearOutEntity });
            return yearOutEntity != null!
                ? Created(new Uri($"{this.ControllerRoute()}/{yearOutEntity.ID}"),
                    yearOutEntity)
                : BadRequest();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
