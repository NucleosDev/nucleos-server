using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Auth.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Auth.Commands;

public class RefreshTokenCommand : IRequest<AuthResponseDto>
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IApplicationDbContext context,
        IJwtGenerator jwtGenerator,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Implementação simplificada - você pode expandir depois
            return new AuthResponseDto
            {
                Success = false,
                Message = "Refresh token não implementado ainda"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar token");
            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro ao renovar token"
            };
        }
    }
}