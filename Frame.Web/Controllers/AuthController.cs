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
            var username = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound("User not found");

            return Ok(new
            {
                user.Username,
                user.Email,
                user.Role
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == dto.Username.ToLower()))
            {
                return BadRequest("Username is taken");
            }

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role,
                PasswordHash = ComputeHash(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                Role = user.Role
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || user.PasswordHash != ComputeHash(dto.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(new AuthResponseDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                Role = user.Role
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
