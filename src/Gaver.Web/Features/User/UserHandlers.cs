using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using MediatR;

namespace Gaver.Web.Features
{
    public class LogInHandler : IRequestHandler<LogInRequest, UserModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public LogInHandler(GaverContext context, IMapperService mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public UserModel Handle(LogInRequest message)
        {
            var name = message.Name;
            var user = context.Users.SingleOrDefault(u => u.Name == name);
            if (user == null)
            {
                user = new User
                {
                    Name = name
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
            var wishList = context.WishLists.FirstOrDefault(l => l.UserId == user.Id);
            if (wishList == null) {
                wishList = new Data.Entities.WishList
                {
                    UserId = user.Id
                };
                context.WishLists.Add(wishList);
                context.SaveChanges();
            }
            return mapper.Map<UserModel>(user);
        }
    }
}