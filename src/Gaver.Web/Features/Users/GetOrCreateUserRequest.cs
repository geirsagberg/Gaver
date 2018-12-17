using Gaver.Data.Entities;
using MediatR;

namespace Gaver.Web.Features.Users
{
    public class GetOrCreateUserRequest : IRequest<User>
    {
        public string PrimaryIdentityId { get; }

        public GetOrCreateUserRequest(string primaryIdentityId)
        {
            PrimaryIdentityId = primaryIdentityId;
        }
    }
}
