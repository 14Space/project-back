using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frame.BusinessLogic.Configurations
{
    public class ProductDescriptionConfiguration : IEntityTypeConfiguration<ProductDescription>
    {
        public void Configure(EntityTypeBuilder<ProductDescription> builder)
        {
            builder.HasKey(pd => pd.Id);

            builder.Property(pd => pd.Description)
                .IsRequired()
                .HasMaxLength(4000);

            builder.HasOne(pd => pd.Advanced)
                .WithOne()
                .HasForeignKey<DescriptionAdvanced>(da => da.DescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
