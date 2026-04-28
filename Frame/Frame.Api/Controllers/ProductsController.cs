using Microsoft.AspNetCore.Mvc;

namespace Frame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // GET /api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        // POST /api/products
        [HttpPost]
        public IActionResult Create()
        {
            return Ok();
        }
    }
}
