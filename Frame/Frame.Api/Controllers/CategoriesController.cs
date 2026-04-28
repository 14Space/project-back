using Microsoft.AspNetCore.Mvc;

namespace Frame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        // GET /api/categories
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        // GET /api/categories/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        // POST /api/categories
        [HttpPost]
        public IActionResult Create()
        {
            return Ok();
        }
    }
}
