using System;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Common.Exceptions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Invitations;
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
        public async Task When_invitation_is_accepted_then_wishListId_and_wishListUserName_is_returned()
        {
            var token = Guid.NewGuid();
            var user = SetupUserWithInvitation(token);

            var response = await TestSubject.Handle(new AcceptInvitationRequest(token));

            response.WishListId.Should().Be(user.WishList.Id);
            response.WishListUserName.Should().Be(user.Name);
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
