using Microsoft.AspNetCore.Mvc;
using eUseControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eUseControl.Web.Controller
{
    [Route(template: "api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // In-memory storage for categories (for demonstration purposes)
        private static List<Category> _categories = new();
        private static int _nextId = 1;

        [HttpGet(template: "all")]
        public IActionResult GetAllCategories()
        {
            return Ok(_categories);
        }

        [HttpGet(template: "{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound(new { Message = $"Category with ID {id} not found" });

            return Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            category.Id = _nextId++;
            category.CreatedAt = DateTime.UtcNow;

            _categories.Add(category);

            return Created($"/api/category/{category.Id}", category);
        }

        [HttpPut(template: "{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            var existingCategory = _categories.FirstOrDefault(c => c.Id == id);

            if (existingCategory == null)
                return NotFound(new { Message = $"Category with ID {id} not found" });

            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

            return Ok(existingCategory);
        }

        [HttpDelete(template: "{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound(new { Message = $"Category with ID {id} not found" });

            _categories.Remove(category);

            return NoContent();
        }
    }
}
