﻿using MediatR;

using Microsoft.AspNetCore.Mvc;

using YaronEfrat.Yiyo.Application.Commands.Mottos;
using YaronEfrat.Yiyo.Application.Models;
using YaronEfrat.Yiyo.Application.Queries;
using YaronEfrat.Yiyo.Domain.Reflection.Models;

namespace YaronEfrat.Yiyo.WebApi.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class MottoController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MottoController> _logger;

    public MottoController(IMediator mediator, ILogger<MottoController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    public async Task<ActionResult<MottoEntity>> Get([FromRoute] int id)
    {
        try
        {
            MottoEntity mottoEntity = await _mediator.Send(new GetMottoQuery {Id = id});
            return mottoEntity != null! ? Ok(mottoEntity) : NotFound();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] MottoEntity mottoEntity)
    {
        try
        {
            mottoEntity = await _mediator.Send(new AddMottoCommand {MottoEntity = mottoEntity});
            return mottoEntity != null!
                ? Created(new Uri($"{this.ControllerRoute()}/{mottoEntity.ID}"),
                    mottoEntity)
                : BadRequest();
        }
        catch (EntityException e)
        {
            _logger.LogError(e.Message);
            return BadRequest();
        }
    }
}
