using AutoMapper;
using Frame.Domain.Entities;
using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Profiles
{
    public class FavoriteProfile : Profile
    {
        public FavoriteProfile()
        {
            CreateMap<Favorite, FavoriteDto>().ReverseMap();
        }
    }
}
