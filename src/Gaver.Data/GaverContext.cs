using System.Text.Json;
using Gaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Data
{
    public class GaverContext : DbContext
    {
        public GaverContext(DbContextOptions<GaverContext> options) : base(options)
        {
        }

        public DbSet<Wish> Wishes { get; set; } = null!;
        public DbSet<WishList> WishLists { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserGroup> UserGroups { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<InvitationToken> InvitationTokens { get; set; } = null!;
        public DbSet<WishOption> WishOptions { get; set; } = null!;
        public DbSet<UserGroupConnection> UserGroupConnections { get; set; } = null!;
        public DbSet<UserFriendConnection> UserFriendConnections { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
                entity.HasIndex(u => u.PrimaryIdentityId).IsUnique();
                entity.HasOne(u => u.WishList).WithOne(wl => wl!.User!);
                entity.HasMany(e => e.Friends).WithOne(e => e.User!);
            });
            modelBuilder.Entity<ChatMessage>(entity => {
                entity.Property(e => e.Created)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NOW()")
                    ;
            });
            modelBuilder.Entity<InvitationToken>(entity => {
                entity.Property(e => e.Created)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NOW()");
            });
            modelBuilder.Entity<UserGroupConnection>(entity => { entity.HasKey(i => new {i.UserGroupId, i.UserId}); });
            modelBuilder.Entity<UserGroup>(entity => {
                entity.HasOne(typeof(User)).WithMany().HasForeignKey(nameof(UserGroup.CreatedByUserId));
            });

            var options = new JsonSerializerOptions {
                IgnoreNullValues = true
            };
            modelBuilder.Entity<WishList>(entity => {
                entity.Property(e => e.WishesOrder).HasConversion(
                    array => JsonSerializer.Serialize(array, options),
                    json => JsonSerializer.Deserialize<int[]>(json, options));
            });

            modelBuilder.Entity<UserFriendConnection>(entity => { entity.HasKey(e => new {e.UserId, e.FriendId}); });
        }
    }
}
