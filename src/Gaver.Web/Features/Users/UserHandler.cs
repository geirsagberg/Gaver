using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Flurl.Http;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Exceptions;
using Gaver.Web.Options;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkQueryableExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace Gaver.Web.Features.Users
{
    public class UserHandler : IRequestHandler<GetUserInfoRequest, CurrentUserDto>,
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
            var user = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(context.Set<User>(),
                u => u.PrimaryIdentityId == request.PrimaryIdentityId, cancellationToken);

            if (user == null) {
                var userInfo = await GetUserInfo(cancellationToken);
                user = new User {
                    Email = userInfo.Email ?? throw new FriendlyException("Email not found"),
                    Name = userInfo.Name ?? throw new FriendlyException("Name not found"),
                    PictureUrl = userInfo.Picture,
                    PrimaryIdentityId = request.PrimaryIdentityId,
                    WishList = new WishList()
                };
                context.Add(user);
                await context.SaveChangesAsync(cancellationToken);
            }

            return user;
        }

        public async Task<CurrentUserDto> Handle(GetUserInfoRequest request, CancellationToken token)
        {
            var userModel = await context.Users.Where(u => u.Id == request.UserId)
                .ProjectTo<CurrentUserDto>(mapper.MapperConfiguration).SingleOrDefaultAsync(token);

            if (userModel == null) {
                throw new FriendlyException("Bruker finnes ikke");
            }

            return userModel;
        }

        public async Task<Unit> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await GetUserInfo(cancellationToken);

            var user = await context.GetOrDieAsync<User>(request.UserId);

            user.Email = userInfo.Email ?? throw new FriendlyException("Email not found");
            user.Name = userInfo.Name ?? throw new FriendlyException("Name not found");
            user.PictureUrl = userInfo.Picture;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<UserInfo> GetUserInfo(CancellationToken token)
        {
            var httpContextItems = httpContextAccessor.HttpContext?.Items ?? throw new DeveloperException("No HttpContext!");
            if (!httpContextItems.ContainsKey("access_token")) {
                throw new HttpException(HttpStatusCode.InternalServerError, "Access token missing");
            }

            var accessToken = httpContextItems["access_token"]?.ToString() ?? throw new FriendlyException("access_token not found");
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
            public string? Email { get; set; }
            public string? Picture { get; set; }
            public string? Nickname { get; set; }
            public string? Name { get; set; }
        }
    }
}
