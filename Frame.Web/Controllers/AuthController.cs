using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Frame.DataAccess;
using Frame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Frame.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role,
                user.LastName,
                user.Phone,
                user.City,
                user.Street
            });
        }

        [HttpDelete("me")]
        [Authorize]
        public async Task<IActionResult> DeleteMe()
        {
            var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.Phone != null) user.Phone = dto.Phone;
            if (dto.City != null) user.City = dto.City;
            if (dto.Street != null) user.Street = dto.Street;

            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Token = _tokenService.CreateToken(user),
                Role = user.Role,
                LastName = user.LastName,
                Phone = user.Phone,
                City = user.City,
                Street = user.Street
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
            {
                return BadRequest("Email is taken");
            }

            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                Role = "User",
                PasswordHash = ComputeHash(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Token = _tokenService.CreateToken(user),
                Role = user.Role,
                LastName = user.LastName,
                Phone = user.Phone,
                City = user.City,
                Street = user.Street
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || user.PasswordHash != ComputeHash(dto.Password))
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Token = _tokenService.CreateToken(user),
                Role = user.Role,
                LastName = user.LastName,
                Phone = user.Phone,
                City = user.City,
                Street = user.Street
            });
        }

        private string ComputeHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
