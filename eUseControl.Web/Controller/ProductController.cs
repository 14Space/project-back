using Microsoft.AspNetCore.Mvc;
using eUseControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eUseControl.Web.Controller
{
    [Route(template: "api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // In-memory storage for products (for demonstration purposes)
        private static List<Product> _products = new();
        private static int _nextId = 1;

        [HttpGet(template: "all")]
        public IActionResult GetAllProducts()
        {
            return Ok(_products);
        }

        [HttpGet(template: "{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { Message = $"Product with ID {id} not found" });

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            product.Id = _nextId++;
            product.CreatedAt = DateTime.UtcNow;

            _products.Add(product);

            return Created($"/api/product/{product.Id}", product);
        }

        [HttpPut(template: "{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
                return NotFound(new { Message = $"Product with ID {id} not found" });

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.CategoryId = updatedProduct.CategoryId;
            existingProduct.Stock = updatedProduct.Stock;

            return Ok(existingProduct);
        }

        [HttpDelete(template: "{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound(new { Message = $"Product with ID {id} not found" });

            _products.Remove(product);

            return NoContent();
        }
    }
}
