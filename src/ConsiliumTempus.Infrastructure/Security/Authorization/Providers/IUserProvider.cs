using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Infrastructure.Security.Authorization.Providers;

internal interface IUserProvider
{
    Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default);
}