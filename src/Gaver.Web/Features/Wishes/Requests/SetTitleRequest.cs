using System.ComponentModel.DataAnnotations;
using Gaver.Web.Features.Wishes.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetTitleRequest : IRequest<WishModel>
    {
        [MinLength(1)]
        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }

    }
}
