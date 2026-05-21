using Frame.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttributesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttributesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttributes([FromQuery] int? categoryId)
        {
            var query = _context.Attributes.AsQueryable();
            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }
            var attributes = await query.ToListAsync();
            var result = attributes.Select(a => new {
                a.Id,
                a.Name,
                a.CategoryId,
                Options = string.IsNullOrEmpty(a.Options) 
                    ? new List<string>() 
                    : System.Text.Json.JsonSerializer.Deserialize<List<string>>(a.Options, (System.Text.Json.JsonSerializerOptions?)null)
            });
            return Ok(result);
        }

        public class CreateAttributeDto
        {
            public string Name { get; set; } = string.Empty;
            public int? CategoryId { get; set; }
            public List<string>? Options { get; set; }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateAttribute([FromBody] CreateAttributeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var attr = new Frame.Domain.Entities.Attribute 
            { 
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Options = dto.Options != null ? System.Text.Json.JsonSerializer.Serialize(dto.Options) : "[]"
            };
            _context.Attributes.Add(attr);
            await _context.SaveChangesAsync();
            return Ok(new { 
                attr.Id, 
                attr.Name, 
                attr.CategoryId, 
                Options = dto.Options ?? new List<string>() 
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateAttribute(int id, [FromBody] CreateAttributeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var attr = await _context.Attributes.FindAsync(id);
            if (attr == null) return NotFound();
            attr.Name = dto.Name;
            attr.CategoryId = dto.CategoryId;
            if (dto.Options != null)
            {
                attr.Options = System.Text.Json.JsonSerializer.Serialize(dto.Options);
            }
            await _context.SaveChangesAsync();
            return Ok(new { 
                attr.Id, 
                attr.Name, 
                attr.CategoryId, 
                Options = dto.Options ?? (string.IsNullOrEmpty(attr.Options) 
                    ? new List<string>() 
                    : System.Text.Json.JsonSerializer.Deserialize<List<string>>(attr.Options, (System.Text.Json.JsonSerializerOptions?)null)) 
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAttribute(int id)
        {
            var attr = await _context.Attributes.FindAsync(id);
            if (attr == null) return NotFound();
            bool isUsed = await _context.ProductAttributeValues.AnyAsync(av => av.AttributeId == id);
            if (isUsed)
                return BadRequest("Нельзя удалить, так как данные используются в товарах");
            _context.Attributes.Remove(attr);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}