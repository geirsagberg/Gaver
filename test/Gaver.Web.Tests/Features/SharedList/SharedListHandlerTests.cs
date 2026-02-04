using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Auth;
using Gaver.Web.Features.SharedList;
using Gaver.Web.Features.SharedList.Requests;
using Xunit;

namespace Gaver.Web.Tests.Features.SharedList;

public class SharedListHandlerTests : DbTestBase<SharedListHandler>
{
    public SharedListHandlerTests()
    {
        // Register real AccessChecker instead of mock so tests can properly check group membership
        Container.Register<IAccessChecker, AccessChecker>();
    }

    [Fact]
    public async Task Can_read_shared_list()
    {
        var bob = new User {
            Name = "Bob",
            PrimaryIdentityId = "Bob"
        };
        var james = new User {
            Name = "James",
            PrimaryIdentityId = "James"
        };
        Context.AddRange(bob, james);
        Context.SaveChanges();

        var result = await TestSubject.Handle(new GetSharedListRequest {
            WishListId = bob.WishList!.Id,
            UserId = james.Id
        });

        result.OwnerUserId.Should().Be(bob.Id);
        result.Users.Should().Contain(u => u.Id == bob.Id);
    }

    [Fact]
    public async Task Group_members_can_see_each_others_wishlists()
    {
        // Arrange
        var alice = new User {
            Name = "Alice",
            PrimaryIdentityId = "Alice"
        };
        var bob = new User {
            Name = "Bob",
            PrimaryIdentityId = "Bob"
        };
        var charlie = new User {
            Name = "Charlie",
            PrimaryIdentityId = "Charlie"
        };
        
        var familyGroup = new UserGroup {
            Name = "Family",
            CreatedByUser = alice,
            Users = { alice, bob, charlie }
        };
        
        Context.AddRange(alice, bob, charlie);
        Context.Add(familyGroup);
        Context.SaveChanges();

        // Act - Bob checks access to Alice's wishlist
        var aliceAccessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = alice.WishList!.Id,
            UserId = bob.Id
        });

        // Act - Charlie checks access to Bob's wishlist
        var bobAccessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = bob.WishList!.Id,
            UserId = charlie.Id
        });

        // Assert
        aliceAccessStatus.Should().Be(ListAccessStatus.Invited);
        bobAccessStatus.Should().Be(ListAccessStatus.Invited);
    }

    [Fact]
    public async Task Non_group_members_cannot_see_wishlists()
    {
        // Arrange
        var alice = new User {
            Name = "Alice",
            PrimaryIdentityId = "Alice"
        };
        var bob = new User {
            Name = "Bob",
            PrimaryIdentityId = "Bob"
        };
        var outsider = new User {
            Name = "Outsider",
            PrimaryIdentityId = "Outsider"
        };
        
        var familyGroup = new UserGroup {
            Name = "Family",
            CreatedByUser = alice,
            Users = { alice, bob }
        };
        
        Context.AddRange(alice, bob, outsider);
        Context.Add(familyGroup);
        Context.SaveChanges();

        // Act - Outsider checks access to Alice's wishlist
        var accessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = alice.WishList!.Id,
            UserId = outsider.Id
        });

        // Assert
        accessStatus.Should().Be(ListAccessStatus.NotInvited);
    }

    [Fact]
    public async Task User_in_multiple_groups_can_see_all_group_members_wishlists()
    {
        // Arrange
        var alice = new User {
            Name = "Alice",
            PrimaryIdentityId = "Alice"
        };
        var bob = new User {
            Name = "Bob",
            PrimaryIdentityId = "Bob"
        };
        var charlie = new User {
            Name = "Charlie",
            PrimaryIdentityId = "Charlie"
        };
        var dave = new User {
            Name = "Dave",
            PrimaryIdentityId = "Dave"
        };
        
        var familyGroup = new UserGroup {
            Name = "Family",
            CreatedByUser = alice,
            Users = { alice, bob }
        };
        
        var friendsGroup = new UserGroup {
            Name = "Friends",
            CreatedByUser = bob,
            Users = { bob, charlie, dave }
        };
        
        Context.AddRange(alice, bob, charlie, dave);
        Context.AddRange(familyGroup, friendsGroup);
        Context.SaveChanges();

        // Act - Bob should see Alice's list (family group)
        var aliceAccessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = alice.WishList!.Id,
            UserId = bob.Id
        });

        // Act - Bob should see Charlie's list (friends group)
        var charlieAccessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = charlie.WishList!.Id,
            UserId = bob.Id
        });

        // Act - Dave should see Bob's list (friends group)
        var bobAccessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = bob.WishList!.Id,
            UserId = dave.Id
        });

        // Act - Alice should NOT see Charlie's list (no shared group)
        var charlieFromAliceAccessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = charlie.WishList!.Id,
            UserId = alice.Id
        });

        // Assert
        aliceAccessStatus.Should().Be(ListAccessStatus.Invited);
        charlieAccessStatus.Should().Be(ListAccessStatus.Invited);
        bobAccessStatus.Should().Be(ListAccessStatus.Invited);
        charlieFromAliceAccessStatus.Should().Be(ListAccessStatus.NotInvited);
    }

    [Fact]
    public async Task Friends_can_still_see_wishlists()
    {
        // Arrange
        var alice = new User {
            Name = "Alice",
            PrimaryIdentityId = "Alice"
        };
        var bob = new User {
            Name = "Bob",
            PrimaryIdentityId = "Bob"
        };
        
        // Set up friendship
        alice.Friends.Add(bob);
        
        Context.AddRange(alice, bob);
        Context.SaveChanges();

        // Act
        var accessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = alice.WishList!.Id,
            UserId = bob.Id
        });

        // Assert
        accessStatus.Should().Be(ListAccessStatus.Invited);
    }

    [Fact]
    public async Task Owner_gets_owner_status()
    {
        // Arrange
        var alice = new User {
            Name = "Alice",
            PrimaryIdentityId = "Alice"
        };
        
        Context.Add(alice);
        Context.SaveChanges();

        // Act
        var accessStatus = await TestSubject.Handle(new CheckSharedListAccessRequest {
            WishListId = alice.WishList!.Id,
            UserId = alice.Id
        });

        // Assert
        accessStatus.Should().Be(ListAccessStatus.Owner);
    }
}