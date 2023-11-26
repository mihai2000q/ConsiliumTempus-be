using ConsiliumTempus.Domain.UserAggregate;

namespace ConsiliumTempus.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
}