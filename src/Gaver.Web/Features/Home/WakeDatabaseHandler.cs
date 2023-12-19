using Gaver.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Home;

public class WakeDatabaseHandler(GaverContext gaverContext) : IRequestHandler<WakeDatabaseRequest> {
    private readonly GaverContext gaverContext = gaverContext;

    public async Task Handle(WakeDatabaseRequest request, CancellationToken cancellationToken) {
        await gaverContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
    }
}
