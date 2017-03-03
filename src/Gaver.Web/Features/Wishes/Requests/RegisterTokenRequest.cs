using System;
using MediatR;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class RegisterTokenRequest : IRequest
    {
        public int WishListId { get; set; }
        public int UserId { get; set; }
        public Guid Token { get; set; }
    }
}