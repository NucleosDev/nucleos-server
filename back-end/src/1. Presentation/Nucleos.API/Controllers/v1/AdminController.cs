using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Admin.Queries;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    public AdminController(IMediator mediator) => _mediator = mediator;

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _mediator.Send(new GetUsersQuery { Page = page, PageSize = pageSize }));

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
        => Ok(await _mediator.Send(new GetDashboardStatsQuery()));
}
