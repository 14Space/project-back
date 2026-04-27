using Microsoft.EntityFrameworkCore;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.User;

namespace eUseControl.Domain.DBContext
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
