using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Feedback
{
    public class SubmitFeedbackRequest : IRequest, IAuthenticatedRequest
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool Anonymous { get; set; }
    }
}
