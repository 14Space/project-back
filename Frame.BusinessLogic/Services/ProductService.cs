using AutoMapper;
using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Frame.DataAccess;
using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frame.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
            
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Reload to get Category name for the DTO
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _mapper.Map(updateProductDto, product);
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
