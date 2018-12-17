using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Flurl.Http;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Constants;
using Gaver.Web.Extensions;
using Gaver.Web.Options;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Users
{
    public class UserHandler : IRequestHandler<GetUserInfoRequest, CurrentUserModel>,
        IRequestHandler<UpdateUserInfoRequest>
    {
        private readonly Auth0Settings auth0Settings;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public UserHandler(GaverContext context, IMapperService mapper, Auth0Settings auth0Settings, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.auth0Settings = auth0Settings;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CurrentUserModel> Handle(GetUserInfoRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
            //var primaryIdentityId = request.User.GetPrimaryIdentityId();
            //var userModel = context.Users.Where(u => u.PrimaryIdentityId == primaryIdentityId)
            //    .ProjectTo<CurrentUserModel>(mapper.MapperConfiguration).SingleOrDefault();
            //if (userModel != null) return userModel;

            
            //var userInfo = await GetUserInfo(token);

            //if (!userInfo.TryGetValue("name", out var name))
            //    throw new FriendlyException(EventIds.MissingName, "Navn mangler");
            //if (!userInfo.TryGetValue("email", out var email))
            //    throw new FriendlyException(EventIds.MissingEmail, "E-post mangler");
            //userInfo.TryGetValue("picture", out var pictureUrl);

            //var user = new User {
            //    PrimaryIdentityId = primaryIdentityId,
            //    Name = name.ToString(),
            //    Email = email.ToString(),
            //    PictureUrl = pictureUrl?.ToString(),
            //    WishLists = {
            //        new WishList()
            //    }
            //};
            //context.Users.Add(user);
            //await context.SaveChangesAsync(token);
            //return mapper.Map<CurrentUserModel>(user);
        }

        private async Task<IDictionary<string, object>> GetUserInfo(CancellationToken token)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            var userInfo = await $"https://{auth0Settings.Domain}/userinfo"
                .WithOAuthBearerToken(accessToken)
                .GetJsonAsync<IDictionary<string, object>>(token);
            return userInfo;
        }

        public async Task<int?> GetUserIdOrNullAsync(string providerId)
        {
            var userId = await context.Users.Where(u => u.PrimaryIdentityId == providerId).Select(u => u.Id)
                .SingleOrDefaultAsync();
            return userId == 0 ? (int?) null : userId;
        }

        public async Task<Unit> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            var userInfo = await GetUserInfo(cancellationToken);



            return Unit.Value;
        }
    }
}
