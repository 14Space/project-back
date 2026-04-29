using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartDto>> GetAllAsync();
        Task<CartDto?> GetByIdAsync(int id);
        Task<CartDto?> GetByUserIdAsync(int userId);
        Task<CartDto> CreateAsync(CartDto cartDto);
        Task<bool> UpdateAsync(int id, CartDto cartDto);
        Task<bool> DeleteAsync(int id);
        
        // Cart Items
        Task<CartItemDto> AddItemAsync(int cartId, CartItemDto itemDto);
        Task<bool> RemoveItemAsync(int itemId);
    }
}
