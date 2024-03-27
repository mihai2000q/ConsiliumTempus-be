using ConsiliumTempus.Domain.Common.Entities;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> Get(string token, CancellationToken cancellationToken);

    Task Add(RefreshToken refreshToken, CancellationToken cancellationToken);
}