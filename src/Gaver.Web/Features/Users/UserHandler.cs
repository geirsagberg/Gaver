using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Logic.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Users
{
    public class GetUserInfoRequest
    {
        [Required]
        public string AccessToken { get; set; }

        [JsonIgnore]
        public string ProviderId { get; set; }
    }

    public class UserHandler : IAsyncRequestHandler<GetUserInfoRequest, UserModel>
    {
        private readonly GaverContext context;
        private readonly Auth0Settings auth0Settings;
        private readonly IMapperService mapper;

        public UserHandler(GaverContext context, IMapperService mapper, Auth0Settings auth0Settings)
        {
            this.context = context;
            this.mapper = mapper;
            this.auth0Settings = auth0Settings;
        }

        public async Task<UserModel> HandleAsync(GetUserInfoRequest request)
        {
            var user = await context.Users.Where(u => u.PrimaryIdentityId == request.ProviderId)
                .Include(u => u.WishLists)
                .SingleOrDefaultAsync();
            if (user == null) {
                var result = await $"https://{auth0Settings.Domain}/userinfo"
                    .WithOAuthBearerToken(request.AccessToken)
                    .GetJsonAsync();
                var userInfo = result as IDictionary<string, object>;
                if (userInfo == null)
                    throw new FriendlyException(EventIds.AuthenticationError, "Noe gikk galt ved innloggingen");

                object name, email;
                if (!userInfo.TryGetValue("name", out name))
                    throw new FriendlyException(EventIds.MissingName, "Navn mangler");
                if (!userInfo.TryGetValue("email", out email))
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
            }
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