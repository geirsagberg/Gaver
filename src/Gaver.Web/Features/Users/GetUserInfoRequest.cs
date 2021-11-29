using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Users;

public class GetUserInfoRequest : IRequest<CurrentUserDto>, IAuthenticatedRequest
{
    public int UserId { get; set; }
}