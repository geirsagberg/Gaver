using Gaver.Data.Entities;
using MediatR;

namespace Gaver.Web.Features.Users;

public class GetOrCreateUserRequest(string primaryIdentityId) : IRequest<User> {
    public string PrimaryIdentityId { get; } = primaryIdentityId;
}
