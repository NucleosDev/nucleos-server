using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Blocos.Commands;
using Nucleos.Application.Features.Blocos.Queries;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BlocosController : ControllerBase
{
    private readonly IMediator _mediator;
    public BlocosController(IMediator mediator) => _mediator = mediator;

    [HttpGet("nucleo/{nucleoId}")]
    public async Task<IActionResult> GetByNucleo(Guid nucleoId)
        => Ok(await _mediator.Send(new GetBlocosByNucleoQuery { NucleoId = nucleoId }));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new GetBlocoByIdQuery { Id = id }));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlocoCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBlocoCommand cmd)
    {
        cmd.Id = id;
        return Ok(await _mediator.Send(cmd));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBlocoCommand { Id = id });
        return NoContent();
    }

    [HttpPut("reorder")]
    public async Task<IActionResult> Reorder([FromBody] ReorderBlocosCommand cmd)
    {
        await _mediator.Send(cmd);
        return NoContent();
    }
}
