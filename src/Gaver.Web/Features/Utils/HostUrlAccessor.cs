using Gaver.Common.Attributes;
using Gaver.Common.Exceptions;

namespace Gaver.Web.Features.Utils;

[SingletonService]
public class HostUrlAccessor(IHttpContextAccessor httpContextAccessor) : IHostUrlAccessor {
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    public string GetHostUrl() {
        var request = httpContextAccessor.HttpContext?.Request ?? throw new DeveloperException("No HttpContext!");

        return request.Scheme + "://" + request.Host;
    }
}
