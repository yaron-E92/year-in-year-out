using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

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
}
