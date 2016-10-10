using AutoMapper.Execution;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;

namespace Gaver.Web.Features.Wishes
{
    public class DeleteWishRequest
    {
        public int WishId { get; set; }
        public int WishListId { get; set; }
    }

    public class DeleteWishHandler : IRequestHandler<DeleteWishRequest>
    {
        private readonly GaverContext context;

        public DeleteWishHandler(GaverContext context)
        {
            this.context = context;
        }

        public void Handle(DeleteWishRequest message)
        {
            context.Delete<Wish>(message.WishId);
            context.SaveChanges();
        }
    }
}