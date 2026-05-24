using Frame.BusinessLogic.DTOs;
using Frame.DataAccess;
using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frame.BusinessLogic.Services
{
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;

        public BannerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BannerDto>> GetAllAsync()
        {
            var banners = await _context.Banners
                .OrderBy(b => b.Order)
                .ToListAsync();

            return banners.Select(b => new BannerDto
            {
                Id = b.Id,
                ImageUrl = b.ImageUrl,
                Link = b.Link,
                Order = b.Order
            });
        }

        public async Task<BannerDto> GetByIdAsync(int id)
        {
            var b = await _context.Banners.FindAsync(id);
            if (b == null) return null;

            return new BannerDto
            {
                Id = b.Id,
                ImageUrl = b.ImageUrl,
                Link = b.Link,
                Order = b.Order
            };
        }

        public async Task<BannerDto> CreateAsync(CreateBannerDto dto, string imageUrl)
        {
            var banner = new Banner
            {
                ImageUrl = imageUrl,
                Link = dto.Link ?? string.Empty,
                Order = dto.Order
            };

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return new BannerDto
            {
                Id = banner.Id,
                ImageUrl = banner.ImageUrl,
                Link = banner.Link,
                Order = banner.Order
            };
        }

        public async Task UpdateAsync(int id, CreateBannerDto dto)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) throw new Exception("Banner not found");

            banner.Link = dto.Link ?? string.Empty;
            banner.Order = dto.Order;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
            }
        }
    }
}
