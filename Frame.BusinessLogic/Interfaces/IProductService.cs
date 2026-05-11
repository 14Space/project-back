using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto createProductDto);
        Task<bool> UpdateAsync(int id, CreateProductDto updateProductDto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filter);
    }
}
