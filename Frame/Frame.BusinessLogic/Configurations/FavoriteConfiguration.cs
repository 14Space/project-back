using Frame.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frame.BusinessLogic.Configurations
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.AddedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Favorite -> User (Cascade — удаление пользователя удаляет его избранное)
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Favorite -> Product (Cascade — удаление товара удаляет его из избранного)
            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
