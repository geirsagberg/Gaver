using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using MediatR;

namespace Gaver.Web.Features
{
    public class LogInRequestHandler : IRequestHandler<LogInRequest, UserModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public LogInRequestHandler(GaverContext context, IMapperService mapper)
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
            return mapper.Map<User, UserModel>(user);
        }
    }
}