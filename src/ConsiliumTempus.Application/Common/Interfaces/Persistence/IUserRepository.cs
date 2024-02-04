using ConsiliumTempus.Domain.UserAggregate;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);

    Task Add(User user);
}