using ConsiliumTempus.Domain.User;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<UserAggregate?> GetUserByEmail(string email);

    Task Add(UserAggregate userAggregate);
}