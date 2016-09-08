using Gaver.Data.Entities;
using MediatR;

namespace Gaver.Web.Features
{
    public class LogInRequest : IRequest<UserModel> {
	public string Name { get; set; }
    }
}