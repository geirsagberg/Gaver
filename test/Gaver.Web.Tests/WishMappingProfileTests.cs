using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.Logic.Services;
using Gaver.TestUtils;
using Gaver.Web.Features.Wishes;
using Gaver.Web.Features.Wishes.Models;
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

            var model = mapperService.Map<InvitationModel>(invitation);

            model.ShouldBeEquivalentTo(new InvitationModel {
                WishListUserName = "OwnerMan",
                WishListId = 3
            });
        }
    }
}