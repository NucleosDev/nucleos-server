using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.ItensLista.Commands;
using Nucleos.Application.Features.ItensLista.DTOs;
using Nucleos.Application.Features.ItensLista.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ItensListaController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItensListaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("lista/{listaId}")]
    public async Task<IActionResult> GetByLista(Guid listaId)
    {
        var result = await _mediator.Send(new GetItensByListaQuery { ListaId = listaId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetItemByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateItemCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteItemCommand { Id = id });
        return NoContent();
    }

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> Toggle(Guid id)
    {
        await _mediator.Send(new ToggleItemCheckedCommand { Id = id });
        return NoContent();
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkUpdate([FromBody] BulkUpdateItemsCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}