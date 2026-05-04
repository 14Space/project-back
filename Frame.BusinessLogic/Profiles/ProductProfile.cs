using AutoMapper;
using Frame.Domain.Entities;
using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Mapping from Product Entity to ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Mapping from CreateProductDto to Product Entity
            CreateMap<CreateProductDto, Product>();
        }
    }
}
