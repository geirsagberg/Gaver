using Gaver.Common.Attributes;
using Gaver.Common.Exceptions;

namespace Gaver.Web.Features.Utils;

[SingletonService]
public class HostUrlAccessor : IHostUrlAccessor
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public HostUrlAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string GetHostUrl()
    {
        var request = httpContextAccessor.HttpContext?.Request ?? throw new DeveloperException("No HttpContext!");

        return request.Scheme + "://" + request.Host;
    }
}
