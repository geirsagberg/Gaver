using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Gaver.Common.Utils;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Invitations;
using Gaver.Web.Features.SharedList;
using Gaver.Web.Features.UserGroups;
using LightInject;
using Xunit;

namespace Gaver.Web.Tests
{
    public class WishMappingProfileTests : TestBase<MapperService>
    {
        public WishMappingProfileTests()
        {
            Container.Register<IEnumerable<Profile>>(factory => new Profile[] {
                factory.Create<WishMappingProfile>()
            });
        }

        [Fact]
        public void Invitation_is_mapped_correctly()
        {
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

            var model = TestSubject.Map<InvitationDto>(invitation);

            model.Should().BeEquivalentTo(new InvitationDto {
                WishListUserName = "OwnerMan",
                WishListId = 3
            });
        }

        [Fact]
        public void UserGroup_is_mapped_correctly()
        {
            var userGroup = new UserGroup {
                Id = 1,
                Name = "Familien",
                CreatedByUserId = 2,
                UserGroupConnections = {
                    new UserGroupConnection {
                        UserId = 3
                    }
                }
            };

            var model = TestSubject.Map<UserGroupDto>(userGroup);

            model.Should().BeEquivalentTo(new UserGroupDto {
                Id = 1,
                Name = "Familien",
                UserIds = {3}
            });
        }
    }
}
