using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Infrastructure.Authorization.Providers;

public interface IUserProvider
{
    Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default);
}