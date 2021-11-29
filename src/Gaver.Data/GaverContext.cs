using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Gaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Data;

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
            entity.HasOne(u => u.WishList).WithOne(wl => wl.User!);
            entity.HasMany(e => e.Friends)
                .WithMany(e => e.FriendsWithMe)
                .UsingEntity<UserFriendConnection>(
                    j => j.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId),
                    j => j.HasOne(c => c.Friend).WithMany().HasForeignKey(c => c.FriendId),
                    j => j.HasKey(e => new { e.UserId, e.FriendId })
                );
            entity.HasMany(e => e.Groups)
                .WithMany(e => e.Users)
                .UsingEntity<UserGroupConnection>(j => j.HasKey(e => new { e.UserGroupId, e.UserId }));
        });

        modelBuilder.Entity<ChatMessage>(entity => {
            if (Database.IsNpgsql()) {
                entity.Property(e => e.Created)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NOW()");
            }

        });
        modelBuilder.Entity<InvitationToken>(entity => {
            if (Database.IsNpgsql()) {
                entity.Property(e => e.Created)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("NOW()");
            }
        });

        modelBuilder.Entity<UserGroup>(entity => {
            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId);
        });

        var options = new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        modelBuilder.Entity<WishList>(entity => {
            entity.Property(e => e.WishesOrder).HasConversion(
                array => JsonSerializer.Serialize(array, options),
                json => JsonSerializer.Deserialize<int[]>(json, options) ?? Array.Empty<int>());
        });

        modelBuilder.Entity<UserFriendConnection>(entity => { entity.HasKey(e => new { e.UserId, e.FriendId }); });
    }
}