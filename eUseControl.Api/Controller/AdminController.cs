using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using eUseControl.Api.Domain;

namespace eUseControl.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // 1. Управление пользователями

        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            // Здесь должна быть логика получения всех пользователей из БД
            return Ok(new[] {
                new User { Id = 1, Username = "User1", Email = "user1@gmail.com" },
                new User { Id = 2, Username = "User2", Email = "user2@gmail.com" }
            });
        }

        [HttpPut("users/{id}/role")]
        public IActionResult ChangeUserRole(int id, [FromBody] string newRole)
        {
            // Логика изменения роли пользователя
            return Ok(new { message = $"Роль пользователя {id} успешно изменена на {newRole}" });
        }

        [HttpDelete("users/{id}")]
        public IActionResult BanUser(int id)
        {
            // Логика удаления/бана пользователя
            return Ok(new { message = $"Пользователь {id} успешно удален/забанен" });
        }


        // 2. Управление товарами

        [HttpPost("products")]
        public ActionResult<Product> AddProduct([FromBody] Product product)
        {
            // Логика добавления нового товара
            product.Id = 999; // генерируем ID
            return CreatedAtAction(nameof(AddProduct), new { id = product.Id }, product);
        }

        [HttpPut("products/{id}")]
        public IActionResult EditProduct(int id, [FromBody] Product product)
        {
            // Логика обновления товара
            return Ok(new { message = $"Товар {id} успешно обновлен" });
        }

        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // Логика удаления товара
            return Ok(new { message = $"Товар {id} успешно удален из каталога" });
        }


        // 3. Статистика

        [HttpGet("stats")]
        public IActionResult GetDashboardStats()
        {
            // Возвращаем сводную информацию
            var stats = new
            {
                TotalUsers = 150,
                TotalProducts = 42,
                TotalSales = 5000.50M
            };
            return Ok(stats);
        }
    }
}
