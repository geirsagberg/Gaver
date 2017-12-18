using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Wishes;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using LightInject;
using Xunit;

namespace Tests
{
    public class WishReaderTests : DbTestBase<WishReader>
    {
        private readonly GaverContext context;

        public WishReaderTests()
        {
            context = Container.Create<GaverContext>();
        }

        [Fact]
        public void Can_read_my_list()
        {
            var mapper = Get<IMapperService>();
            mapper.Profiles.Should().NotBeEmpty();
            var user = new User {
                Name = "Bob",
                WishLists = {
                    new WishList {
                        Title = "My list"
                    }
                }
            };
            context.Add(user);
            context.SaveChanges();

            var result = TestSubject.Handle(new GetMyListRequest {
                UserId = user.Id
            });

            result.Title.Should().Be("My list");
        }

        [Fact]
        public void Can_read_shared_list()
        {
            var bob = new User {
                Name = "Bob",
                WishLists = {
                    new WishList()
                }
            };
            var james = new User {
                Name = "James"
            };
            context.AddRange(bob, james);
            context.SaveChanges();

            var result = TestSubject.Handle(new GetSharedListRequest {
                ListId = bob.WishLists.First().Id,
                UserId = james.Id
            });

            result.Owner.Should().Be("Bob");
        }
    }
}