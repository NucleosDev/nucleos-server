using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Nucleos.Commands;
using Nucleos.Application.Features.Nucleos.DTOs;
using Nucleos.Application.Features.Nucleos.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]

public class NucleosController : ControllerBase
{
    private readonly IMediator _mediator;

    public NucleosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetNucleosQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetNucleoByIdQuery { Id = id });
        return Ok(result);
    }

   [HttpPost]
public async Task<IActionResult> Create([FromBody] CreateNucleoCommand command)
{
    Console.WriteLine("AUTHENTICATED: " + User.Identity?.IsAuthenticated);

    foreach (var claim in User.Claims)
    {
        Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");
    }

    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
}

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNucleoCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteNucleoCommand { Id = id });
        return NoContent();
    }

    [HttpPost("{id}/share")]
    public async Task<IActionResult> Share(Guid id, [FromBody] ShareNucleoCommand command)
    {
        if (id != command.NucleoId)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();


    }
}