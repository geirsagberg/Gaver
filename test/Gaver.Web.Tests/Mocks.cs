using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Gaver.Web.Tests;

public class Mocks
{
    public static IHttpContextAccessor GetMockHttpContextAccessor()
    {
        var httpRequest = Substitute.ForPartsOf<HttpRequest>();
        httpRequest.Scheme = "http";
        httpRequest.Host = new HostString("localhost");
        var httpContext = Substitute.ForPartsOf<HttpContext>();
        httpContext.Request.Returns(httpRequest);
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(httpContext);
        return httpContextAccessor;
    }
}