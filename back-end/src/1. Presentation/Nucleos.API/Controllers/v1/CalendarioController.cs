using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Calendario.Commands;
using Nucleos.Application.Features.Calendario.DTOs;
using Nucleos.Application.Features.Calendario.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CalendarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("nucleo/{nucleoId}")]
    public async Task<IActionResult> GetByNucleo(Guid nucleoId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var query = new GetEventosByNucleoQuery
        {
            NucleoId = nucleoId,
            Start = start,
            End = end
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetEventoByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventoCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEventoCommand { Id = id });
        return NoContent();
    }
}