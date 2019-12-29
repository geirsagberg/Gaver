using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Common.Exceptions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Invitations;
using Gaver.Web.Tests.Extensions;
using Xunit;

namespace Gaver.Web.Tests.Features.Invitations
{
    public class InvitationHandlerTests : DbTestBase<InvitationHandler>
    {
        [Fact]
        public void Cannot_accept_invitation_to_own_list()
        {
            var token = Guid.NewGuid();
            var user = SetupUserWithInvitation(token);

            Func<Task> action = () => TestSubject.Handle(new AcceptInvitationRequest(token) {
                UserId = user.Id
            });

            action.Should().Throw<FriendlyException>()
                .WithMessage("Du kan ikke godta en invitasjon til din egen liste.");
        }

        [Fact]
        public async Task When_invitation_is_accepted_then_wishListId_and_userName_is_returned()
        {
            var token = Guid.NewGuid();
            var user = SetupUserWithInvitation(token);
            var otherUser = new User {
                Name = "Bob",
                WishList = new WishList()
            };
            Context.Add(otherUser);
            Context.SaveChanges();

            var response = await TestSubject.Handle(new AcceptInvitationRequest(token) {
                UserId = otherUser.Id
            });

            response.WishListId.Should().Be(user.WishList.Id);
            response.UserName.Should().Be(user.Name);
        }

        [Fact]
        public async Task When_invitation_is_accepted_then_UserFriendConnections_are_created_both_ways()
        {
            var token = Guid.NewGuid();
            var bob = new User {
                Name = "Bob",
                WishList = new WishList()
            };
            var alice = new User {
                Name = "Alice",
                WishList = new WishList {
                    InvitationTokens = {
                        new InvitationToken {
                            Token = token
                        }
                    }
                }
            };
            Context.AddRange(alice, bob);
            Context.SaveChanges();

            await TestSubject.Handle(new AcceptInvitationRequest(token) {
                UserId = bob.Id
            });

            Context.Reset();
            Context.UserFriendConnections.Select(u => new { u.UserId, u.FriendId }).Should().BeEquivalentTo(
                new {
                    UserId = alice.Id,
                    FriendId = bob.Id
                }, new {
                    UserId = bob.Id,
                    FriendId = alice.Id
                });
        }

        private User SetupUserWithInvitation(Guid token)
        {
            var user = new User {
                Name = "Geir",
                WishList = new WishList {
                    InvitationTokens = {
                        new InvitationToken {
                            Token = token
                        }
                    }
                }
            };
            Context.Add(user);
            Context.SaveChanges();
            return user;
        }
    }
}
