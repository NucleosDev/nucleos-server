using Microsoft.AspNetCore.Mvc;

namespace Nucleos.API.Controllers;

/// <summary>
/// Health check endpoint - NÃO VERSIONADO
/// Este endpoint é usado por ferramentas de monitoramento (Kubernetes, Docker, etc.)
/// Deve funcionar independentemente da versão da API
/// </summary>
[ApiController]
[Route("api/[controller]")]  // Note: SEM "v1" na rota!
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new 
        { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            message = "API is running"
        });
    }
}
