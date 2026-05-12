using AutoMapper;
using Frame.Domain.Entities;
using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId));

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
                .ForMember(dest => dest.Attributes, opt => opt.MapFrom(src => src.AttributeValues));

            CreateMap<ProductAttributeValue, ProductAttributeResponseDto>()
                .ForMember(dest => dest.AttributeName, opt => opt.MapFrom(src => src.Attribute.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
        }
    }
}