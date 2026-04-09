using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Features.Auth.Commands;
using Nucleos.Application.Features.Auth.DTOs;
using Nucleos.Infrastructure.Persistence.Context;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    private readonly NucleosDbContext _context;

    public AuthController(
        IMediator mediator,
        ILogger<AuthController> logger,
        NucleosDbContext context)
    {
        _mediator = mediator;
        _logger = logger;
        _context = context;
    }

    // =========================
    // REGISTER
    // =========================
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponseDto
            {
                Success = false,
                Message = "Dados inválidos",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        var result = await _mediator.Send(new RegisterCommand
        {
            Email = request.Email,
            FullName = request.FullName,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword,
            Phone = request.Phone,
            Cpf = request.Cpf
        });

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // =========================
    // LOGIN
    // =========================
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new AuthResponseDto
            {
                Success = false,
                Message = "Dados inválidos"
            });
        }

        var result = await _mediator.Send(new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        });

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    // =========================
    // LOGOUT
    // =========================
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logout realizado com sucesso" });
    }

    // =========================
    // 👤 ME (CORRETO)
    // =========================
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        _logger.LogInformation("Authorization Header: {AuthHeader}", authHeader);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            _logger.LogWarning("❌ Claim NameIdentifier não encontrada");
            return Unauthorized(new { message = "Token inválido (sem claim)" });
        }

        if (!Guid.TryParse(userIdClaim, out Guid userId))
        {
            _logger.LogWarning("❌ Claim inválida: {UserIdClaim}", userIdClaim);
            return Unauthorized(new { message = "Token inválido (formato inválido)" });
        }

        var user = await _context.Users
            .Include(u => u.Profile)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogWarning("❌ Usuário não encontrado: {UserId}", userId);
            return NotFound(new { message = "Usuário não encontrado" });
        }

        return Ok(new
        {
            userId = user.Id,
            email = user.Email,
            fullName = user.Profile?.FullName,
            cpf = user.Cpf,
            phone = user.Phone,
            nickname = user.Profile?.Nickname,
            avatarUrl = user.Profile?.AvatarUrl,
            emailVerified = user.EmailVerified,
            active = user.Active,
            createdAt = user.CreatedAt
        });
    }
}