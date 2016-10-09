using System;
using System.Net.Http;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes
{
    public class SetUrlRequest : IRequest<WishModel>
    {
        public string Url { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }
    }

    public class SetUrlHandler : IRequestHandler<SetUrlRequest, WishModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public SetUrlHandler(GaverContext context, IMapperService mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public WishModel Handle(SetUrlRequest message)
        {
            var wish = context.GetOrDie<Wish>(message.WishId);
            if (wish.WishListId != message.WishListId)
                throw new FriendlyException(EventIds.WrongList, $"Wish {message.WishId} does not belong to list {message.WishListId}");

            var urlString = message.Url;

            if (urlString.IsNullOrEmpty())
            {
                wish.Url = null;
            }
            else
            {
                if (!urlString.StartsWith("http"))
                    urlString = $"http://{urlString}";

                Uri uri;
                if (!Uri.TryCreate(urlString, UriKind.Absolute, out uri))
                    throw new FriendlyException(EventIds.InvalidUrl, "Ugyldig lenke");

                wish.Url = uri.ToString();
            }

            context.SaveChanges();

            return mapper.Map<WishModel>(wish);
        }
    }
}