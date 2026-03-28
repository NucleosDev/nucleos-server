using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Listas.Commands;
using Nucleos.Application.Features.Listas.DTOs;
using Nucleos.Application.Features.Listas.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ListasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("bloco/{blocoId}")]
    public async Task<IActionResult> GetByBloco(Guid blocoId)
    {
        var result = await _mediator.Send(new GetListasByBlocoQuery { BlocoId = blocoId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetListaByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListaCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateListaCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteListaCommand { Id = id });
        return NoContent();
    }
}