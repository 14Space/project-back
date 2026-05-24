using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Services
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerDto>> GetAllAsync();
        Task<BannerDto> GetByIdAsync(int id);
        Task<BannerDto> CreateAsync(CreateBannerDto dto, string imageUrl);
        Task UpdateAsync(int id, CreateBannerDto dto);
        Task DeleteAsync(int id);
    }
}
