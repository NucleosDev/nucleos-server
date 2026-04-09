using Microsoft.AspNetCore.Mvc;

namespace Nucleos.API.Controllers;

/// <summary>
/// Health check endpoint - NÃO VERSIONADO
/// Este endpoint é usado por ferramentas de monitoramento (Kubernetes, Docker, etc.)
/// Deve funcionar independentemente da versão da API
/// </summary>
[ApiController]
[Route("v1/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API OK");
    }
}
