using Gaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Data
{
    public class GaverContext : DbContext
    {
        public GaverContext()
        {

        }

        public GaverContext(DbContextOptions<GaverContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(u => u.Name).IsUnique();
            });
        }

        public DbSet<Wish> Wishes { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
