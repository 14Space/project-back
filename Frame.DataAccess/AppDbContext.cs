using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frame.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Catalog
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        // Specifications / filters
        public DbSet<Frame.Domain.Entities.Attribute> Attributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

        // Users
        public DbSet<User> Users { get; set; }

        // Shopping
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        // Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Trade-In
        public DbSet<TradeInRequest> TradeInRequests { get; set; }

        // Home Page
        public DbSet<Banner> Banners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            // Each Attribute belongs to a Category
            modelBuilder.Entity<Frame.Domain.Entities.Attribute>()
                .HasOne(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Composite PK for attribute values
            modelBuilder.Entity<ProductAttributeValue>()
                .HasKey(pav => new { pav.ProductId, pav.AttributeId });

            // Decimal precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TradeInRequest>()
                .Property(t => t.OfferAmount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
