using Microsoft.AspNetCore.Mvc;
using eUseControl.BusinessLogic.Core;
using System.Collections.Generic;
using eUseControl.Domain.Entities;

namespace eUseControl.Web.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AdminApi _adminApi;

        public ProductsController()
        {
            _adminApi = new AdminApi();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(_adminApi.GetAllProducts());
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            _adminApi.AddProduct(product);
            return Ok(new { message = "Товар успешно создан" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            _adminApi.UpdateProduct(id, product);
            return Ok(new { message = "Товар обновлен" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _adminApi.DeleteProduct(id);
            return Ok(new { message = "Товар удален" });
        }
    }
}
