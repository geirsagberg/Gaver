using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Flurl.Http;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Options;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Users
{
    public class UserHandler : IRequestHandler<GetUserInfoRequest, CurrentUserModel>,
        IRequestHandler<UpdateUserInfoRequest>,
        IRequestHandler<GetOrCreateUserRequest, User>
    {
        private readonly Auth0Settings auth0Settings;
        private readonly GaverContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapperService mapper;

        public UserHandler(GaverContext context, IMapperService mapper, Auth0Settings auth0Settings,
            IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.auth0Settings = auth0Settings;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> Handle(GetOrCreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await context.Set<User>().SingleOrDefaultAsync(u => u.PrimaryIdentityId == request.PrimaryIdentityId, cancellationToken);

            if (user == null) {
                var userInfo = await GetUserInfo(cancellationToken);
                user = new User {
                    Email = userInfo.Email,
                    Name = userInfo.Name,
                    PictureUrl = userInfo.Picture,
                    PrimaryIdentityId = request.PrimaryIdentityId,
                    WishList = new WishList()
                };
                context.Add(user);
                await context.SaveChangesAsync(cancellationToken);
            }

            return user;
        }

        public async Task<CurrentUserModel> Handle(GetUserInfoRequest request, CancellationToken token)
        {
            var userModel = await context.Users.Where(u => u.Id == request.UserId)
                .ProjectTo<CurrentUserModel>(mapper.MapperConfiguration).SingleOrDefaultAsync(token);

            if (userModel == null) {
                throw new FriendlyException("Bruker finnes ikke");
            }

            return userModel;
        }

        public async Task<Unit> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await GetUserInfo(cancellationToken);

            var user = await context.GetOrDieAsync<User>(request.UserId);

            user.Email = userInfo.Email;
            user.Name = userInfo.Name;
            user.PictureUrl = userInfo.Picture;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<UserInfo> GetUserInfo(CancellationToken token)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var userInfo = await $"https://{auth0Settings.Domain}/userinfo"
                .WithOAuthBearerToken(accessToken)
                .GetJsonAsync<UserInfo>(token);
            return userInfo;
        }

        public async Task<int?> GetUserIdOrNullAsync(string providerId)
        {
            var userId = await context.Users.Where(u => u.PrimaryIdentityId == providerId).Select(u => u.Id)
                .SingleOrDefaultAsync();
            return userId == 0 ? (int?) null : userId;
        }

        public class UserInfo
        {
            public string Email { get; set; }
            public string Picture { get; set; }
            public string Nickname { get; set; }
            public string Name { get; set; }
        }
    }
}
