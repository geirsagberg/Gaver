using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gaver.Web.Features.WishList
{
    public class ShareListRequest : IAsyncRequest {
        [Required]
        [MinLength(1)]
        public string[] Emails { get; set; }
    }
}