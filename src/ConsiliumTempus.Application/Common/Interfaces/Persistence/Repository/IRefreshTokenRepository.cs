using ConsiliumTempus.Domain.Authentication;
using ConsiliumTempus.Domain.Authentication.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> Get(RefreshTokenId id, CancellationToken cancellationToken = default);

    Task Add(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}