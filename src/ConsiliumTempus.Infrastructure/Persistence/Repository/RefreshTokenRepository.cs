using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Common.Entities;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class RefreshTokenRepository(ConsiliumTempusDbContext dbContext) : IRefreshTokenRepository
{
    public Task<RefreshToken?> Get(string id, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<RefreshToken>()
            .Include(rt => rt.User)
            .SingleOrDefaultAsync(rt => rt.Id == new Guid(id), cancellationToken);
    }

    public async Task Add(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
    }
}