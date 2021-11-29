using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities;

public class User : IEntityWithId
{
    public int Id { get; set; }

    [MaxLength(255)]
    [Required]
    public string PrimaryIdentityId { get; init; } = "";

    [MaxLength(40)]
    [Required]
    public string Name { get; set; } = "";

    [MaxLength(255)]
    [Required]
    public string Email { get; set; } = "";

    [MaxLength(255)]
    public string? PictureUrl { get; set; }

    public WishList? WishList { get; init; } = new();

    public ICollection<Wish> BoughtWishes { get; set; } = new HashSet<Wish>();
    public ICollection<UserGroup> Groups { get; set; } = new HashSet<UserGroup>();

    // Joining entity; used to simplify editing
    public IEnumerable<UserGroupConnection> UserGroupConnections { get; set; } = new HashSet<UserGroupConnection>();
    public ICollection<User> Friends { get; set; } = new HashSet<User>();

    // Reverse mapping of many-to-many relationship
    public ICollection<User> FriendsWithMe { get; set; } = new HashSet<User>();
}