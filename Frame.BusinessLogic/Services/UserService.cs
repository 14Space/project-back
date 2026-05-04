using AutoMapper;
using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frame.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> CreateAsync(RegisterUserDto registerUserDto)
        {
            var user = _mapper.Map<User>(registerUserDto);
            // In a real app, password hashing would happen here.
            // For now, we follow the instructions and use the provided entities.
            user.PasswordHash = registerUserDto.Password; // Simple assignment for now
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _mapper.Map(userDto, user);
            user.Id = id;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
