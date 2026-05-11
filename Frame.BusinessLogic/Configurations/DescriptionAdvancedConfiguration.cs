using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frame.BusinessLogic.Configurations
{
    public class DescriptionAdvancedConfiguration : IEntityTypeConfiguration<DescriptionAdvanced>
    {
        public void Configure(EntityTypeBuilder<DescriptionAdvanced> builder)
        {
            builder.HasKey(da => da.Id);

            builder.Property(da => da.Brand)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(da => da.H).IsRequired();
            builder.Property(da => da.W).IsRequired();
            builder.Property(da => da.L).IsRequired();
        }
    }
}
