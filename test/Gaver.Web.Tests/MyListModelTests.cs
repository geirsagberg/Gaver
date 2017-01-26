using AutoMapper;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.Logic.Services;
using Gaver.TestUtils;
using Gaver.Web.Features.Wishes;
using Gaver.Web.Features.Wishes.Models;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace Gaver.Web.Tests
{
    public class MyListModelTests : TestBase
    {
        [Fact]
        public void Mapping_works()
        {
            var httpRequest = Substitute.For<HttpRequest>();
            httpRequest.Scheme = "http";
            httpRequest.Host = new HostString("localhost");
            var httpContext = Substitute.For<HttpContext>();
            httpContext.Request.Returns(httpRequest);
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);
            Container.RegisterInstance(httpContextAccessor);
            Container.Register<Profile, WishMappingProfile>();

            var mapperService = Container.Create<MapperService>();

            var wishList = new WishList {
                Id = 3,
                Invitations = {
                    new Invitation {
                        User = new User {
                            Name = "Geir"
                        }
                    }
                }
            };
            var model = mapperService.Map<MyListModel>(wishList);
            model.ShouldBeEquivalentTo(new MyListModel {
                Invitations = {
                    new InvitationModel {
                        UserName = "Geir",
                        WishListUrl = "http://localhost/list/3"
                    }
                }
            });
        }
    }
}