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
            modelBuilder.Entity<WishList>(entity => {
                entity.HasMany(wl => wl.Wishes).WithOne(w => w.WishList).HasForeignKey(w => w.WishListId).IsRequired();
            });
        }

        public DbSet<Wish> Wishes { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
