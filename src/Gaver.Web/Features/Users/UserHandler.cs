using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;

using Gaver.Web.Constants;
using Gaver.Web.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Users
{
    public class GetUserInfoRequest : IRequest<UserModel>
    {
        [Required]
        public string AccessToken { get; set; }

        [JsonIgnore]
        public string ProviderId { get; set; }
    }

    public class UserHandler : IAsyncRequestHandler<GetUserInfoRequest, UserModel>
    {
        private readonly Auth0Settings auth0Settings;
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public UserHandler(GaverContext context, IMapperService mapper, Auth0Settings auth0Settings)
        {
            this.context = context;
            this.mapper = mapper;
            this.auth0Settings = auth0Settings;
        }

        public async Task<UserModel> Handle(GetUserInfoRequest request)
        {
            var user = await context.Users.Where(u => u.PrimaryIdentityId == request.ProviderId)
                .Include(u => u.WishLists)
                .SingleOrDefaultAsync();
            if (user != null) return mapper.Map<UserModel>(user);

            var result = await $"https://{auth0Settings.Domain}/userinfo"
                .WithOAuthBearerToken(request.AccessToken)
                .GetJsonAsync();
            if (!(result is IDictionary<string, object> userInfo))
                throw new FriendlyException(EventIds.AuthenticationError, "Noe gikk galt ved innloggingen");

            if (!userInfo.TryGetValue("name", out var name))
                throw new FriendlyException(EventIds.MissingName, "Navn mangler");
            if (!userInfo.TryGetValue("email", out var email))
                throw new FriendlyException(EventIds.MissingEmail, "E-post mangler");

            user = new User {
                PrimaryIdentityId = request.ProviderId,
                Name = name.ToString(),
                Email = email.ToString(),
                WishLists = {
                    new WishList()
                }
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return mapper.Map<UserModel>(user);
        }

        public async Task<int?> GetUserIdOrNullAsync(string providerId)
        {
            var userId = await context.Users.Where(u => u.PrimaryIdentityId == providerId).Select(u => u.Id)
                .SingleOrDefaultAsync();
            return userId == 0 ? (int?) null : userId;
        }
    }
}