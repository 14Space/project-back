using AutoMapper;
using Frame.Domain.Entities;
using Frame.BusinessLogic.DTOs;

namespace Frame.BusinessLogic.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
        }
    }
}
