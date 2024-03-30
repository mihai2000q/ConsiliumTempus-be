using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Common.Interfaces.Security;

public interface ICurrentUserProvider
{
    Task<UserAggregate?> GetCurrentUser(CancellationToken cancellationToken = default);
}