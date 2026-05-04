using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frame.BusinessLogic.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
