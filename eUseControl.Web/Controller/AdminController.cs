using Microsoft.AspNetCore.Mvc;
using eUseControl.BusinessLogic.Core;
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminApi _adminApi;

        public AdminController()
        {
            _adminApi = new AdminApi();
        }

        [HttpGet("products")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_adminApi.GetAllProducts());
        }

        [HttpPost("products")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            _adminApi.AddProduct(product);
            return Ok(new { message = "Товар успешно создан" });
        }

        [HttpPut("products/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            _adminApi.UpdateProduct(id, product);
            return Ok(new { message = "Товар обновлен" });
        }

        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _adminApi.DeleteProduct(id);
            return Ok(new { message = "Товар удален" });
        }

        [HttpPut("users/{id}/role")]
        public IActionResult UpdateRole(int id, [FromQuery] string role)
        {
            _adminApi.UpdateUserRole(id, role);
            return Ok(new { message = "Роль пользователя изменена" });
        }

        [HttpDelete("users/{id}")]
        public IActionResult BanUser(int id)
        {
            _adminApi.DeleteUser(id);
            return Ok(new { message = "Пользователь забанен" });
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            return Ok(new { TotalUsers = 10, TotalProducts = 5, TotalSales = 1500 });
        }
    }
}
