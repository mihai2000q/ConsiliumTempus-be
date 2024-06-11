using ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;
using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;
using ConsiliumTempus.Infrastructure.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace ConsiliumTempus.Infrastructure.Persistence.Repository;

public sealed class RefreshTokenRepository(ConsiliumTempusDbContext dbContext) : IRefreshTokenRepository
{
    public Task<RefreshToken?> Get(RefreshTokenId id, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<RefreshToken>()
            .Include(rt => rt.History.OrderByDescending(h => h. CreatedDateTime))
            .Include(rt => rt.User)
            .SingleOrDefaultAsync(rt => rt.Id == id, cancellationToken);
    }

    public async Task Add(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await dbContext.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
    }
}