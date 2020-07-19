using System.Threading;
using System.Threading.Tasks;
using Gaver.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Home
{
    public class WakeDatabaseHandler : AsyncRequestHandler<WakeDatabaseRequest>
    {
        private readonly GaverContext gaverContext;

        public WakeDatabaseHandler(GaverContext gaverContext)
        {
            this.gaverContext = gaverContext;
        }

        protected override async Task Handle(WakeDatabaseRequest request, CancellationToken cancellationToken)
        {
            await gaverContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
        }
    }
}
