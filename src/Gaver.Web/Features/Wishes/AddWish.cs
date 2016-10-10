using System.ComponentModel.DataAnnotations;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes
{
    public class AddWishRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
    }

    public class AddWishHandler : IRequestHandler<AddWishRequest, WishModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public AddWishHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public WishModel Handle(AddWishRequest message)
        {
            var wishListId = context.Users.Where(u => u.Name == message.UserName).Select(u => u.WishLists.Single().Id).Single();
            var wish = new Wish
            {
                Title = message.Title,
                WishListId = wishListId
            };
            context.Add(wish);
            context.SaveChanges();
            return mapperService.Map<WishModel>(wish);
        }
    }
}