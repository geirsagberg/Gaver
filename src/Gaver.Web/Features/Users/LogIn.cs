using System.ComponentModel.DataAnnotations;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;

namespace Gaver.Web.Features
{
    public class LogInRequest
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }

    public class LogInHandler : IRequestHandler<LogInRequest, LoginUserModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public LogInHandler(GaverContext context, IMapperService mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public LoginUserModel Handle(LogInRequest message)
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
                wishList = new WishList
                {
                    UserId = user.Id
                };
                context.WishLists.Add(wishList);
                context.SaveChanges();
            }
            var userModel = mapper.Map<LoginUserModel>(user);
            userModel.WishListId = wishList.Id;
            return userModel;
        }
    }
}