using Gaver.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Home;

public class WakeDatabaseHandler(GaverContext gaverContext) : IRequestHandler<WakeDatabaseRequest> {
    public async Task Handle(WakeDatabaseRequest request, CancellationToken cancellationToken) => await gaverContext.Database.ExecuteSqlRawAsync("SELECT 1", cancellationToken);
}
