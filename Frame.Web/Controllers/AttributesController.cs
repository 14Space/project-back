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
        public async Task<IActionResult> GetAttributes()
        {
            var attributes = await _context.Attributes
                .Select(a => new { a.Id, a.Name })
                .ToListAsync();
            return Ok(attributes);
        }

        public class CreateAttributeDto
        {
            public string Name { get; set; } = string.Empty;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateAttribute([FromBody] CreateAttributeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var attr = new Frame.Domain.Entities.Attribute { Name = dto.Name };
            _context.Attributes.Add(attr);
            await _context.SaveChangesAsync();
            return Ok(new { attr.Id, attr.Name });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateAttribute(int id, [FromBody] CreateAttributeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var attr = await _context.Attributes.FindAsync(id);
            if (attr == null) return NotFound();
            attr.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(new { attr.Id, attr.Name });
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