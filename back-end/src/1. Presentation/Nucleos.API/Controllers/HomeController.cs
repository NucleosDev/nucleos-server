using Microsoft.AspNetCore.Mvc;

namespace Nucleos.API.Controllers;

[ApiController]
[Route("home")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            name = "Nucleos API",
            version = "1.0.0",
            status = "running",
            environment = "Development",
            timestamp = DateTime.UtcNow,
            swagger = "/swagger",
            health = "/api/health",
            versionInfo = "/api/v1/version",
            endpoints = new string[]
            {
                "GET /swagger - Documentacao interativa",
                "GET /api/health - Health check",
                "GET /api/v1/version - Versao da API",
                "POST /api/v1/auth/login - Autenticacao",
                "POST /api/v1/auth/register - Registro",
                "GET /api/v1/nucleos - Lista nucleos",
                "POST /api/v1/nucleos - Cria nucleo",
                "GET /api/v1/blocos - Lista blocos",
                "GET /api/v1/listas - Lista listas",
                "GET /api/v1/tarefas - Lista tarefas",
                "GET /api/v1/habitos - Lista habitos"
            },
            features = new
            {
                auth = "Em desenvolvimento",
                nucleos = "Em desenvolvimento",
                blocos = "Em desenvolvimento",
                listas = "Em desenvolvimento",
                tarefas = "Em desenvolvimento",
                habitos = "Em desenvolvimento",
                gamification = "Em desenvolvimento"
            },
            howToRun = "dotnet run --urls=http://localhost:5000"
        });
    }
}