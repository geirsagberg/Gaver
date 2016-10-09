using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes
{
    public class SetBoughtRequest : IRequest<WishModel>
    {
        public bool IsBought { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
        [JsonIgnore]
        public int WishId { get; set; }
        [JsonIgnore]
        public string UserName { get; set; }
    }
    

    public class SetBoughtHandler : IRequestHandler<SetBoughtRequest, WishModel>
    {
        private readonly IMapperService mapper;
        private readonly GaverContext context;

        public SetBoughtHandler(IMapperService mapper, GaverContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public WishModel Handle(SetBoughtRequest message)
        {
            var wish = context.GetOrDie<Wish>(message.WishId);
            if (wish.WishListId != message.WishListId)
                throw new FriendlyException(EventIds.WrongList, $"Wish {message.WishId} does not belong to list {message.WishListId}");

            var userId = context.Set<User>().Single(u => u.Name == message.UserName).Id;

            if (wish.BoughtByUserId != null && wish.BoughtByUserId != userId)
                throw new FriendlyException(EventIds.AlreadyBought, "Wish has already been bought by someone else");

            if (message.IsBought)
            {
                wish.BoughtByUserId = userId;
            }
            else
            {
                wish.BoughtByUserId = null;
            }
            context.SaveChanges();
            return mapper.Map<WishModel>(wish);
        }
    }
}