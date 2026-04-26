using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using eUseControl.Api.Domain;

namespace eUseControl.Api.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private static List<Product> _products = new();
    private static int _nextId = 1;

    [HttpGet("all")]
    public IActionResult GetAllProducts()
    {
        return Ok(_products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound(new { Message = "Product with ID " + id + " not found" });
        }
        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return Created("", product);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        _products.Remove(product);
        return NoContent();
    }
}