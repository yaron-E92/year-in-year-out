using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

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
}
