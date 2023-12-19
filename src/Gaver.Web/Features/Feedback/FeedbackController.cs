using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Feedback;

public class FeedbackController(IMediator mediator) : GaverControllerBase {
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task SubmitFeedback(SubmitFeedbackRequest request) => await mediator.Send(request);
}
