using Frame.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _context.Brands
                .Select(b => new { b.Id, b.Name })
                .ToListAsync();
            return Ok(brands);
        }

        public class CreateBrandDto
        {
            public string Name { get; set; } = string.Empty;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var brand = new Frame.Domain.Entities.Brand { Name = dto.Name };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return Ok(new { brand.Id, brand.Name });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] CreateBrandDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return BadRequest("Name is required");
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            brand.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(new { brand.Id, brand.Name });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return NotFound();
            bool isUsed = await _context.Products.AnyAsync(p => p.BrandId == id);
            if (isUsed)
                return BadRequest("Нельзя удалить, так как данные используются в товарах");
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}