using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDto>> GetAllAsync();
        Task<IEnumerable<FavoriteDto>> GetByUserIdAsync(int userId);
        Task<FavoriteDto> CreateAsync(FavoriteDto favoriteDto);
        Task<bool> DeleteAsync(int id);
    }
}
