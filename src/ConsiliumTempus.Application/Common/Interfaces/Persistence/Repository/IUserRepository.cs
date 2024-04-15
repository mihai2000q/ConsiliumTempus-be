using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Domain.User.ValueObjects;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence.Repository;

public interface IUserRepository
{
    Task<UserAggregate?> Get(UserId id, CancellationToken cancellationToken = default);

    Task<UserAggregate?> GetByEmail(string email, CancellationToken cancellationToken = default);

    Task Add(UserAggregate user, CancellationToken cancellationToken = default);

    void Remove(UserAggregate user);
}