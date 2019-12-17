using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.UserGroups
{
    public class UserGroupHandler : IRequestHandler<GetMyUserGroupsRequest, UserGroupsDto>,
        IRequestHandler<CreateUserGroupRequest, UserGroupDto>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public UserGroupHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public async Task<UserGroupDto> Handle(CreateUserGroupRequest request, CancellationToken cancellationToken)
        {
            var userGroup = new UserGroup {
                Name = request.Name,
                CreatedByUserId = request.UserId,
                UserGroupConnections = {
                    new UserGroupConnection {
                        UserId = request.UserId
                    }
                }
            };
            context.Add(userGroup);
            await context.SaveChangesAsync(cancellationToken);
            return mapperService.Map<UserGroupDto>(userGroup);
        }

        public async Task<UserGroupsDto> Handle(GetMyUserGroupsRequest request, CancellationToken cancellationToken)
        {
            var groups = await context.UserGroupConnections.Where(c => c.UserId == request.UserId)
                .Select(u => u.UserGroup)
                .ProjectTo<UserGroupDto>(mapperService.MapperConfiguration).ToListAsync(cancellationToken);
            return new UserGroupsDto {
                UserGroups = groups
            };
        }
    }
}
