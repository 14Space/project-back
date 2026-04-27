using Microsoft.AspNetCore.Mvc;
using eUseControl.BusinessLogic.Core;
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AdminApi _adminApi;

        public CategoriesController()
        {
            _adminApi = new AdminApi();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            return Ok(_adminApi.GetAllCategories());
        }

        [HttpPost]
        public IActionResult AddCategory([FromBody] Category category)
        {
            _adminApi.AddCategory(category);
            return Ok(new { message = "Категория добавлена" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            _adminApi.UpdateCategory(id, category);
            return Ok(new { message = "Категория обновлена" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _adminApi.DeleteCategory(id);
            return Ok(new { message = "Категория удалена" });
        }
    }
}
