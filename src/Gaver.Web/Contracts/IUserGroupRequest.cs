namespace Gaver.Web.Contracts;

public interface IUserGroupRequest : IAuthenticatedRequest
{
    int UserGroupId { get; }
}