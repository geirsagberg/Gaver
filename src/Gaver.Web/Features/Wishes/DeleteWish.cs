using AutoMapper.Execution;
using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;

namespace Gaver.Web.Features.Wishes
{
    public class DeleteWishRequest : IRequest
    {
        public int WishId { get; set; }
        public int WishListId { get; set; }
    }

    public class DeleteWishHandler : IRequestHandler<DeleteWishRequest, Unit>
    {
        private readonly GaverContext context;

        public DeleteWishHandler(GaverContext context)
        {
            this.context = context;
        }

        public Unit Handle(DeleteWishRequest message)
        {
            context.Delete<Wish>(message.WishId);
            context.SaveChanges();
            return Unit.Value;
        }
    }
}