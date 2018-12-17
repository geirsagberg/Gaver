using System;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class RegisterTokenRequest : IRequest, IAuthenticatedRequest
    {
        public int WishListId { get; set; }
        public int UserId { get; set; }
        public Guid Token { get; set; }
    }
}
