using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Flurl.Http;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Gaver.Web.Features.Users
{
    public class GetUserInfoRequest
    {
        public int UserId { get; set; }
    }

    public class UserHandler :
        IAsyncRequestHandler<GetUserInfoRequest, LoginUserModel>
    {
        private readonly GaverContext context;
        private readonly Auth0Settings auth0Settings;
        private readonly IMapperService mapper;

        public UserHandler(GaverContext context, IMapperService mapper, IOptions<Auth0Settings> options)
        {
            this.context = context;
            this.auth0Settings = options.Value;
            this.mapper = mapper;
        }

        public async Task<UserModel> EnsureUserAsync(string providerId, string idToken)
        {
            var user = context.Users.SingleOrDefault(u => u.PrimaryIdentityId == providerId);
            if (user == null)
            {
                dynamic userInfo = await $"https://{auth0Settings.Domain}/tokeninfo".PostJsonAsync(new
                {
                    id_token = idToken
                }).ReceiveJson();
                string name = userInfo.name;
                string email = userInfo.email;
                user = new User
                {
                    PrimaryIdentityId = providerId,
                    Name = name,
                    Email = email
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            return mapper.Map<UserModel>(user);
        }

        public async Task<LoginUserModel> HandleAsync(GetUserInfoRequest request)
        {
            var user = await context.Users.Where(u => u.Id == request.UserId)
                .ProjectTo<LoginUserModel>(mapper.MapperConfiguration)
                .SingleOrDefaultAsync();
            if (user == null)
                throw new FriendlyException(EventIds.UnknownUserId, "Ukjent bruker");
            user.WishListId = await context.WishLists.Where(wl => wl.UserId == request.UserId)
                .Select(wl => wl.Id)
                .FirstOrDefaultAsync();
            return user;
        }
    }
}