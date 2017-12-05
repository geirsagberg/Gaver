﻿using System.Linq;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Chat;
using Xunit;

namespace Gaver.Web.Tests
{
    public class GetMessagesTests : DbTestBase<GetMessagesHandler>
    {
        [Fact]
        public void Can_get_chatMessages()
        {
            var firstWishList = new WishList {
                User = new User(),
                ChatMessages = {
                    new ChatMessage {
                        Text = "Hello",
                        User = new User()
                    }
                }
            };
            var secondWishList = new WishList {
                User = new User(),
                ChatMessages = {
                    new ChatMessage {
                        User = new User(),
                        Text = "Whatup"
                    }
                }
            };
            Context.AddRange(firstWishList, secondWishList);
            Context.SaveChanges();

            var result = TestSubject.Handle(new GetMessagesRequest {
                UserId = 1,
                WishListId = firstWishList.Id
            });

            result.Messages.Select(m => m.Text).ShouldAllBeEquivalentTo(new[] {
                "Hello"
            });
        }
    }
}