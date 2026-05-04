using AutoMapper;
using Frame.Domain.Entities;
using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
