using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Gaver.Common.Utils;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Invitations;
using Gaver.Web.Features.SharedList;
using LightInject;
using Xunit;

namespace Gaver.Web.Tests
{
    public class WishMappingProfileTests : TestBase
    {
        [Fact]
        public void Invitation_is_mapped_correctly()
        {
            Container.Register<IEnumerable<Profile>>(factory => new Profile[] {
                factory.Create<WishMappingProfile>()
            });
            var mapperService = Container.Create<MapperService>();
            var invitation = new Invitation {
                WishListId = 3,
                UserId = 2,
                User = new User {
                    Name = "Geir"
                },
                WishList = new WishList {
                    User = new User {
                        Name = "OwnerMan"
                    }
                }
            };

            var model = mapperService.Map<InvitationDto>(invitation);

            model.Should().BeEquivalentTo(new InvitationDto {
                WishListUserName = "OwnerMan",
                WishListId = 3
            });
        }
    }
}