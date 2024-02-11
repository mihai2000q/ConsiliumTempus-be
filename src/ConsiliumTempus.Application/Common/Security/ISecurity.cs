using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Common.Security;

public interface ISecurity
{
    Task<UserAggregate> GetUserFromToken(string plainToken, CancellationToken cancellationToken = default);
}