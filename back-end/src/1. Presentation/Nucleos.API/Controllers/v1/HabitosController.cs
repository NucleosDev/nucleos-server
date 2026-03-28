using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Habitos.Commands;
using Nucleos.Application.Features.Habitos.DTOs;
using Nucleos.Application.Features.Habitos.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class HabitosController : ControllerBase
{
    private readonly IMediator _mediator;

    public HabitosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("bloco/{blocoId}")]
    public async Task<IActionResult> GetByBloco(Guid blocoId)
    {
        var result = await _mediator.Send(new GetHabitosByBlocoQuery { BlocoId = blocoId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetHabitoByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("{id}/progresso")]
    public async Task<IActionResult> GetProgresso(Guid id)
    {
        var result = await _mediator.Send(new GetHabitoProgressoQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHabitoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHabitoCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteHabitoCommand { Id = id });
        return NoContent();
    }

    [HttpPost("{id}/registrar")]
    public async Task<IActionResult> Registrar(Guid id, [FromBody] RegistrarHabitoCommand command)
    {
        if (id != command.HabitoId)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }
}