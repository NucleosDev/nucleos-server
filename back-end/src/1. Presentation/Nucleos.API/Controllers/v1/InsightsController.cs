using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.IA.Commands;
using Nucleos.Application.Features.IA.DTOs;
using Nucleos.Application.Features.IA.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class InsightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InsightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetInsightsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetInsightByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost("gerar")]
    public async Task<IActionResult> Gerar([FromBody] GerarInsightCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{id}/aplicar")]
    public async Task<IActionResult> Aplicar(Guid id)
    {
        await _mediator.Send(new AplicarInsightCommand { Id = id });
        return NoContent();
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] EnviarMensagemCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}