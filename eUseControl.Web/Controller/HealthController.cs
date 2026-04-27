using Microsoft.AspNetCore.Mvc;

namespace eUseControl.Web.Controller
{
    [Route(template: "api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet(template: "ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
    }
}
