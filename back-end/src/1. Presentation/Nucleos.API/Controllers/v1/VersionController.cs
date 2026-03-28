using Microsoft.AspNetCore.Mvc;

namespace Nucleos.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            version = "1.0.0",
            name = "Nucleos API",
            status = "operational",
            endpoints = new[]
            {
                "auth/login",
                "auth/register",
                "nucleos",
                "blocos",
                "listas",
                "tarefas",
                "habitos",
                "gamificacao",
                "insights"
            }
        });
    }
}
