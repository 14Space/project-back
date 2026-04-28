using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Frame.BusinessLogic
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Products and catalog
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductDescription> ProductDescriptions { get; set; }
        public DbSet<DescriptionAdvanced> DescriptionAdvanceds { get; set; }

        // Users
        public DbSet<User> Users { get; set; }

        // Shopping
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        // Personalization
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<CompareItem> CompareItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically applies all IEntityTypeConfiguration<T> classes from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
