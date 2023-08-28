using MediatR;

using Microsoft.AspNetCore.Mvc;

using YaronEfrat.Yiyo.Application.Commands.PersonalEvents;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries.PersonalEvents;
using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.WebApi.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class PersonalEventController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PersonalEventController> _logger;

    public PersonalEventController(IMediator mediator, ILogger<PersonalEventController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<PersonalEventEntity>> Get([FromRoute]int id)
    {
        try
        {
            PersonalEventEntity personalEventEntity = await _mediator.Send(new GetPersonalEventQuery {Id = id});
            return personalEventEntity != null! ? Ok(personalEventEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PersonalEventEntity personalEventEntity)
    {
        try
        {
            personalEventEntity = await _mediator.Send(new AddPersonalEventCommand { PersonalEventEntity = personalEventEntity });
            return personalEventEntity != null!
                ? Created(new Uri($"{this.ControllerRoute()}/{personalEventEntity.ID}"),
                    personalEventEntity)
                : BadRequest();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
