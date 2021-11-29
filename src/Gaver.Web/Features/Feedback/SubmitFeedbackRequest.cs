using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Feedback;

public class SubmitFeedbackRequest : IRequest, IAuthenticatedRequest
{
    public int UserId { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(4000)]
    public string? Message { get; set; } = "";
    public bool Anonymous { get; set; }
}