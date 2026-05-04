using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET /api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET /api/categories/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = $"Category with ID {id} not found" });
            }
            return Ok(category);
        }

        // POST /api/categories
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var result = await _categoryService.CreateAsync(categoryDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT /api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto categoryDto)
        {
            var success = await _categoryService.UpdateAsync(id, categoryDto);
            if (!success) return NotFound();
            
            return NoContent();
        }

        // DELETE /api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryService.DeleteAsync(id);
            if (!success) return NotFound();
            
            return NoContent();
        }
    }
}
