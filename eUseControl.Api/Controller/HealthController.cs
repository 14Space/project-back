using Microsoft.AspNetCore.Mvc;

namespace eUseControl.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
