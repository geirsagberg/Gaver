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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(u => u.PrimaryIdentityId).IsUnique();
            });
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.Property(e => e.Created)
                    .ValueGeneratedOnAdd()
                    .ForNpgsqlHasDefaultValueSql("NOW()")
                    ;
            });
            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasKey(i => new {i.WishListId, i.UserId});
            });
            modelBuilder.Entity<InvitationToken>(entity => {
                entity.Property(e => e.Created)
                    .ValueGeneratedOnAdd()
                    .ForNpgsqlHasDefaultValueSql("NOW()");
            });
        }

        public DbSet<Wish> Wishes { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<InvitationToken> InvitationTokens { get; set; }
    }
}