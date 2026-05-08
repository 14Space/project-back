using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frame.DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Available");

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // N:1 Product -> Category  (Restrict: удаление категории невозможно, пока есть товары)
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1:N Product -> ProductImage  (Cascade: удаление товара удаляет все его картинки)
            builder.HasMany(p => p.Images)
                .WithOne()
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1:1 Product -> ProductDescription  (Cascade)
            builder.HasOne(p => p.Description)
                .WithOne()
                .HasForeignKey<ProductDescription>(pd => pd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
