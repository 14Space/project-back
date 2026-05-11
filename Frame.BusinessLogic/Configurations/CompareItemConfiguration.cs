using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frame.BusinessLogic.Configurations
{
    public class CompareItemConfiguration : IEntityTypeConfiguration<CompareItem>
    {
        public void Configure(EntityTypeBuilder<CompareItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.AddedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
