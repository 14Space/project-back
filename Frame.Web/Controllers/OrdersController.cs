using Frame.DataAccess;
using Frame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return Unauthorized();

            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Images)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders.Select(o => new {
                o.Id,
                o.OrderDate,
                o.TotalPrice,
                o.Status,
                Items = o.Items.Select(i => new {
                    i.ProductId,
                    i.Product.Name,
                    i.Quantity,
                    i.Price,
                    Image = i.Product.Images.FirstOrDefault() != null ? i.Product.Images.First().Url : (string?)null
                })
            }));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return Ok(orders.Select(o => new {
                o.Id,
                o.UserId,
                o.OrderDate,
                o.TotalPrice,
                o.Status,
                Items = o.Items.Select(i => new { i.ProductId, i.Product.Name, i.Quantity, i.Price })
            }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                return BadRequest("No products provided.");
            }

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return Unauthorized();

            var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            if (!products.Any())
            {
                return BadRequest("Products not found.");
            }

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                Status = "Pending"
            };

            decimal totalPrice = 0;

            foreach (var productId in productIds)
            {
                var product = products.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    order.Items.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = 1,
                        Price = product.Price
                    });
                    totalPrice += product.Price;
                }
            }

            order.TotalPrice = totalPrice;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order created successfully", OrderId = order.Id, TotalPrice = order.TotalPrice });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status)) return BadRequest("Status is required");

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound("Order not found");

            order.Status = dto.Status;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Order status updated successfully", Status = order.Status });
        }
    }

    public class OrderStatusUpdateDto
    {
        public string Status { get; set; }
    }
}
