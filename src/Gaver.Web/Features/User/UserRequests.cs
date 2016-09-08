using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gaver.Web.Features
{
    public class LogInRequest : IRequest<UserModel>
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}