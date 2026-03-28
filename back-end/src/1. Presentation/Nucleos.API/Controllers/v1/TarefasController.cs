using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Tarefas.Commands;
using Nucleos.Application.Features.Tarefas.DTOs;
using Nucleos.Application.Features.Tarefas.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class TarefasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarefasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("bloco/{blocoId}")]
    public async Task<IActionResult> GetByBloco(Guid blocoId)
    {
        var result = await _mediator.Send(new GetTarefasByBlocoQuery { BlocoId = blocoId });
        return Ok(result);
    }

    [HttpGet("vencendo")]
    public async Task<IActionResult> GetVencendo()
    {
        var result = await _mediator.Send(new GetTarefasVencendoQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTarefaByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTarefaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTarefaCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteTarefaCommand { Id = id });
        return NoContent();
    }

    [HttpPost("{id}/concluir")]
    public async Task<IActionResult> Concluir(Guid id)
    {
        await _mediator.Send(new ConcluirTarefaCommand { Id = id });
        return NoContent();
    }
}