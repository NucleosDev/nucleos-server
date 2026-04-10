using MediatR;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;


public class AuthorizationBehaviour<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationBehaviour(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (request is not IAuthorizableRequest authRequest)
            return await next();

        // Verifica se usuário está autenticado
        if (!_currentUserService.UserId.HasValue)
            throw new UnauthorizedException();

        // Verifica permissão
        if (!string.IsNullOrWhiteSpace(authRequest.Permission) &&
            !_currentUserService.HasPermission(authRequest.Permission))
        {
            throw new ForbiddenException();
        }

        // Verifica acesso ao próprio recurso (owner check)
        if (authRequest.UserId.HasValue &&
            authRequest.UserId != _currentUserService.UserId)
        {
            throw new ForbiddenException();
        }

        return await next();
    }
}