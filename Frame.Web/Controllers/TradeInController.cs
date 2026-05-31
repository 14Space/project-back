using Frame.DataAccess;
using Frame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Frame.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TradeInController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TradeInController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyRequests()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var requests = await _context.TradeInRequests
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.Date)
                .Select(r => new
                {
                    r.Id,
                    r.UserId,
                    r.Category,
                    r.Description,
                    r.Status,
                    r.OfferAmount,
                    r.Date
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _context.TradeInRequests
                .Include(r => r.User)
                .OrderByDescending(r => r.Date)
                .Select(r => new
                {
                    r.Id,
                    r.UserId,
                    Username = r.User.Username,
                    r.Category,
                    r.Description,
                    r.Status,
                    r.OfferAmount,
                    r.Date
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpGet("{id}/photos")]
        public async Task<IActionResult> GetRequestPhotos(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var photosJson = await _context.TradeInRequests
                .Where(r => r.Id == id)
                .Select(r => new { r.PhotosJson, r.UserId })
                .FirstOrDefaultAsync();

            if (photosJson == null) return NotFound();
            if (photosJson.UserId != user.Id && !User.IsInRole("Admin") && !User.IsInRole("Manager"))
                return Forbid();

            return Ok(JsonSerializer.Deserialize<List<string>>(photosJson.PhotosJson));
        }

        [HttpGet("{id}/details")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetRequestDetails(int id)
        {
            var request = await _context.TradeInRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            return Ok(new
            {
                request.Id,
                request.UserId,
                Username = request.User.Username,
                request.Category,
                request.Description,
                Photos = JsonSerializer.Deserialize<List<string>>(request.PhotosJson),
                request.Status,
                request.OfferAmount,
                request.Date
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateTradeInDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var request = new TradeInRequest
            {
                UserId = user.Id,
                Category = dto.Category,
                Description = dto.Description,
                PhotosJson = JsonSerializer.Serialize(dto.Photos ?? new List<string>()),
                Status = "pending",
                Date = DateTime.UtcNow
            };

            _context.TradeInRequests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                request.Id,
                request.UserId,
                request.Category,
                request.Description,
                Photos = JsonSerializer.Deserialize<List<string>>(request.PhotosJson),
                request.Status,
                request.OfferAmount,
                request.Date
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] UpdateTradeInDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var request = await _context.TradeInRequests.FindAsync(id);
            if (request == null) return NotFound();

            // Пользователь может только принять/отклонить свою заявку
            // Админ/менеджер может менять статус и оценку любой заявки
            bool isOwner = request.UserId == user.Id;
            bool isAdmin = User.IsInRole("Admin") || User.IsInRole("Manager");

            if (!isOwner && !isAdmin) return Forbid();

            if (isOwner && !isAdmin)
            {
                // Пользователь может только accepted/rejected
                if (dto.Status != "accepted" && dto.Status != "rejected") return Forbid();
                if (dto.OfferAmount.HasValue) return Forbid();
            }

            if (dto.Status != null) request.Status = dto.Status;
            if (dto.OfferAmount.HasValue) request.OfferAmount = dto.OfferAmount;

            await _context.SaveChangesAsync();
            return Ok(new { request.Id, request.Status, request.OfferAmount });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var request = await _context.TradeInRequests.FindAsync(id);
            if (request == null) return NotFound();

            if (request.UserId != user.Id && !User.IsInRole("Admin") && !User.IsInRole("Manager"))
                return Forbid();

            _context.TradeInRequests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class CreateTradeInDto
    {
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string>? Photos { get; set; }
    }

    public class UpdateTradeInDto
    {
        public string? Status { get; set; }
        public decimal? OfferAmount { get; set; }
    }
}
