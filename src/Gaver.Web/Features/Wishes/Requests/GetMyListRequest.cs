using Gaver.Web.Features.Wishes.Models;
using MediatR;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class GetMyListRequest : IRequest<MyListModel>
    {
        public int UserId { get; set; }
    }
}