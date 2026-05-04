using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(CategoryDto categoryDto);
        Task<bool> UpdateAsync(int id, CategoryDto categoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
