using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace Gaver.Web.Utils
{
    public class CustomSignalROptionsSetup : IConfigureOptions<SignalROptions>
    {
        public void Configure(SignalROptions options)
        {
            options.RegisterInvocationAdapter<CustomJsonNetInvocationAdapter>("json");
        }
    }
}