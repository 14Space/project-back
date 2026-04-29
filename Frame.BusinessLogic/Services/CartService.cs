using AutoMapper;
using Frame.BusinessLogic.DTOs;
using Frame.BusinessLogic.Interfaces;
using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frame.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartDto>> GetAllAsync()
        {
            var carts = await _context.Carts.Include(c => c.Items).ToListAsync();
            return _mapper.Map<IEnumerable<CartDto>>(carts);
        }

        public async Task<CartDto?> GetByIdAsync(int id)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto?> GetByUserIdAsync(int userId)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> CreateAsync(CartDto cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> UpdateAsync(int id, CartDto cartDto)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return false;

            _mapper.Map(cartDto, cart);
            cart.Id = id;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return false;

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItemDto> AddItemAsync(int cartId, CartItemDto itemDto)
        {
            var item = _mapper.Map<CartItem>(itemDto);
            item.CartId = cartId;
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return _mapper.Map<CartItemDto>(item);
        }

        public async Task<bool> RemoveItemAsync(int itemId)
        {
            var item = await _context.CartItems.FindAsync(itemId);
            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
