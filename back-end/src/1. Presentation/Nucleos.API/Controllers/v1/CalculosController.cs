using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Calculos.Commands;
using Nucleos.Application.Features.Calculos.Queries;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CalculosController : ControllerBase
{
    private readonly IMediator _mediator;
    public CalculosController(IMediator mediator) => _mediator = mediator;

    [HttpGet("bloco/{blocoId}/config")]
    public async Task<IActionResult> GetConfig(Guid blocoId)
        => Ok(await _mediator.Send(new GetCalculoConfigQuery { BlocoId = blocoId }));

    [HttpGet("bloco/{blocoId}/resultado")]
    public async Task<IActionResult> GetResultado(Guid blocoId)
        => Ok(await _mediator.Send(new GetCalculoResultadoQuery { BlocoId = blocoId }));

    [HttpPost("configurar")]
    public async Task<IActionResult> Configurar([FromBody] ConfigurarCalculoCommand cmd)
        => Ok(await _mediator.Send(cmd));

    [HttpPost("executar")]
    public async Task<IActionResult> Executar([FromBody] ExecutarCalculoCommand cmd)
        => Ok(await _mediator.Send(cmd));
}
