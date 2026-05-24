using Frame.DataAccess;
using AutoMapper;
using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
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
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.AttributeValues)
                    .ThenInclude(av => av.Attribute)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.AttributeValues)
                    .ThenInclude(av => av.Attribute)
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == createProductDto.CategoryName);
            if (category == null)
            {
                category = new Category { Name = createProductDto.CategoryName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                OldPrice = createProductDto.OldPrice,
                Status = createProductDto.Status,
                Description = createProductDto.Description ?? string.Empty,
                CategoryId = category.Id,
                BrandId = createProductDto.BrandId,
                SubcategoryName = createProductDto.SubcategoryName
            };

            foreach (var imgUrl in createProductDto.Images)
            {
                product.Images.Add(new ProductImage { Url = imgUrl });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            foreach (var attr in createProductDto.Attributes)
            {
                _context.ProductAttributeValues.Add(new ProductAttributeValue
                {
                    ProductId = product.Id,
                    AttributeId = attr.AttributeId,
                    Value = attr.Value
                });
            }
            await _context.SaveChangesAsync();

            await _context.Entry(product).Reference(p => p.Category).LoadAsync();
            await _context.Entry(product)
                .Collection(p => p.AttributeValues)
                .Query()
                .Include(av => av.Attribute)
                .LoadAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto updateProductDto)
        {
            var product = await _context.Products
                .Include(p => p.AttributeValues)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return false;
            _mapper.Map(updateProductDto, product);

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == updateProductDto.CategoryName);
            if (category == null)
            {
                category = new Category { Name = updateProductDto.CategoryName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            product.CategoryId = category.Id;

            // Удаляем старые атрибуты и записываем новые
            _context.ProductAttributeValues.RemoveRange(product.AttributeValues);

            foreach (var attr in updateProductDto.Attributes)
            {
                _context.ProductAttributeValues.Add(new ProductAttributeValue
                {
                    ProductId = product.Id,
                    AttributeId = attr.AttributeId,
                    Value = attr.Value
                });
            }

            // Удаляем старые изображения и записываем новые
            _context.ProductImages.RemoveRange(product.Images);
            product.Images.Clear();
            foreach (var imgUrl in updateProductDto.Images)
            {
                product.Images.Add(new ProductImage { Url = imgUrl });
            }

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

        public async Task<List<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.AttributeValues)
                    .ThenInclude(av => av.Attribute)
                .AsQueryable();

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
                query = query.Where(p => p.Category.Name == filter.CategoryName);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);

            if (filter.OnlyAvailable.HasValue && filter.OnlyAvailable.Value)
                query = query.Where(p => p.Status == "Available");

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                query = query.Where(p => p.Name.Contains(filter.SearchTerm));

            if (filter.Attributes != null && filter.Attributes.Any())
            {
                var attributeGroups = filter.Attributes
                    .Select(a => {
                        var parts = a.Split(':', 2);
                        return new { AttrId = int.Parse(parts[0]), Value = parts[1] };
                    })
                    .GroupBy(x => x.AttrId)
                    .ToList();

                foreach (var group in attributeGroups)
                {
                    var attrId = group.Key;
                    var allowedValues = group.Select(g => g.Value).ToList();
                    
                    query = query.Where(p => p.AttributeValues.Any(av => av.AttributeId == attrId && allowedValues.Contains(av.Value)));
                }
            }

            var products = await query.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}