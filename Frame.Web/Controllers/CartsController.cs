using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var cart = await _cartService.GetByUserIdAsync(userId);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartDto cartDto)
        {
            var result = await _cartService.CreateAsync(cartDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CartDto cartDto)
        {
            var success = await _cartService.UpdateAsync(id, cartDto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _cartService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddItem(int cartId, [FromBody] CartItemDto itemDto)
        {
            var result = await _cartService.AddItemAsync(cartId, itemDto);
            return Ok(result);
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var success = await _cartService.RemoveItemAsync(itemId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
