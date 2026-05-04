using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(RegisterUserDto registerUserDto);
        Task<bool> UpdateAsync(int id, UserDto userDto);
        Task<bool> DeleteAsync(int id);
    }
}
