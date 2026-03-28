using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nucleos.Application.Features.Gamificacao.Commands;
using Nucleos.Application.Features.Gamificacao.DTOs;
using Nucleos.Application.Features.Gamificacao.Queries;
using System.Threading.Tasks;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class GamificacaoController : ControllerBase
{
    private readonly IMediator _mediator;

    public GamificacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("level")]
    public async Task<IActionResult> GetLevel()
    {
        var result = await _mediator.Send(new GetUserLevelQuery());
        return Ok(result);
    }

    [HttpGet("conquistas")]
    public async Task<IActionResult> GetConquistas()
    {
        var result = await _mediator.Send(new GetUserConquistasQuery());
        return Ok(result);
    }

    [HttpGet("streaks")]
    public async Task<IActionResult> GetStreaks()
    {
        var result = await _mediator.Send(new GetUserStreaksQuery());
        return Ok(result);
    }

    [HttpPost("add-xp")]
    public async Task<IActionResult> AddXp([FromBody] AdicionarXPCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("atualizar-streak")]
    public async Task<IActionResult> AtualizarStreak([FromBody] AtualizarStreakCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("desbloquear-conquista")]
    public async Task<IActionResult> DesbloquearConquista([FromBody] DesbloquearConquistaCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
}