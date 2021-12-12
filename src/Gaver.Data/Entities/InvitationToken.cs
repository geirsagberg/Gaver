using System;
using Gaver.Data.Contracts;

namespace Gaver.Data.Entities;

public class InvitationToken : IEntityWithId
{
    public int Id { get; set; }
    public Guid Token { get; set; } = Guid.NewGuid();
    public DateTimeOffset Created { get; set; }
    public int WishListId { get; set; }

    public WishList? WishList { get; set; }
}
