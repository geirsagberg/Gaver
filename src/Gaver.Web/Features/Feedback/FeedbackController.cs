using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Feedback;

public class FeedbackController : GaverControllerBase
{
    private readonly IMediator mediator;

    public FeedbackController(IMediator mediator) => this.mediator = mediator;

    [HttpPost]
    public async Task SubmitFeedback(SubmitFeedbackRequest request) => await mediator.Send(request);
}