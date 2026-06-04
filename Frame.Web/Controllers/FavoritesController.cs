using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var favorites = await _favoriteService.GetAllAsync();
            return Ok(favorites);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var favorites = await _favoriteService.GetByUserIdAsync(userId);
            return Ok(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteDto favoriteDto)
        {
            var result = await _favoriteService.CreateAsync(favoriteDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _favoriteService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteByProduct(int productId)
        {
            var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var favorites = await _favoriteService.GetByUserIdAsync(userId);
            var toDelete = favorites.FirstOrDefault(f => f.ProductId == productId);
            if (toDelete == null) return NotFound();
            var success = await _favoriteService.DeleteAsync(toDelete.Id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}