using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gaver.Data.Entities;
using MediatR;

namespace Gaver.Web.Features.WishList
{
    public class GetWishesRequest : IRequest<IList<Wish>> {}

    public class ShareListRequest : IAsyncRequest {
        [Required]
        [MinLength(1)]
        public string[] Emails { get; set; }
    }
}