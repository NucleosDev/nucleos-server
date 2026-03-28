using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Features.Auth.Commands;
using Nucleos.Application.Features.Auth.DTOs;
using Nucleos.Infrastructure.Persistence.Context;
using Nucleos.Application.Common.Interfaces;   
using AutoMapper;                             
using Nucleos.Application.Common.Exceptions;  

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    private readonly NucleosDbContext _context; // ← Adicionar o DbContext

    public AuthController(IMediator mediator, ILogger<AuthController> logger, NucleosDbContext context)
    {
        _mediator = mediator;
        _logger = logger;
        _context = context;
    }

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

        var command = new RegisterCommand
        {
            Email = request.Email,
            FullName = request.FullName,
            Password = request.Password,
            ConfirmPassword = request.ConfirmPassword, 
            Phone = request.Phone,
            Cpf = request.Cpf
        };
        
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return BadRequest(result);
            
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
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

        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        };
        
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return Unauthorized(result);
            
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest(new AuthResponseDto
            {
                Success = false,
                Message = "Refresh token é obrigatório"
            });
        }

        var command = new RefreshTokenCommand
        {
            RefreshToken = request.RefreshToken
        };
        
        var result = await _mediator.Send(command);
        
        if (!result.Success)
            return Unauthorized(result);
            
        return Ok(result);
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound();

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
            active = user.Active
        });
    }
}