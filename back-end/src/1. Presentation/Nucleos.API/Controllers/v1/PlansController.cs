using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Admin.Commands;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "admin")]
public class PlansController : ControllerBase
{
    private readonly IMediator _mediator;
    public PlansController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlanCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("subscription")]
    public async Task<IActionResult> ManageSubscription([FromBody] ManageSubscriptionCommand cmd)
        => Ok(await _mediator.Send(cmd));
}
