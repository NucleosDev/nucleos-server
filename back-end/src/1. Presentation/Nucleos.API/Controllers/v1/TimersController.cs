using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Timers.Commands;
using Nucleos.Application.Features.Timers.DTOs;
using Nucleos.Application.Features.Timers.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class TimersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("nucleo/{nucleoId}")]
    public async Task<IActionResult> GetByNucleo(Guid nucleoId)
    {
        var result = await _mediator.Send(new GetTimersByNucleoQuery { NucleoId = nucleoId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTimerByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartTimerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/stop")]
    public async Task<IActionResult> Stop(Guid id)
    {
        var result = await _mediator.Send(new StopTimerCommand { Id = id });
        return Ok(result);
    }

    [HttpPost("{id}/pause")]
    public async Task<IActionResult> Pause(Guid id)
    {
        var result = await _mediator.Send(new PauseTimerCommand { Id = id });
        return Ok(result);
    }

    [HttpPost("{id}/resume")]
    public async Task<IActionResult> Resume(Guid id)
    {
        var result = await _mediator.Send(new ResumeTimerCommand { Id = id });
        return Ok(result);
    }
}